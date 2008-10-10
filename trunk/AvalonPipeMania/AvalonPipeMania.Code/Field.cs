using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;

namespace AvalonPipeMania.Code
{
	[Script]
	public class Field
	{
		public readonly Canvas Container;

		public readonly Canvas Pipes;

		public readonly TileField Tiles;

		public Color DefaultPipeColor = Colors.Yellow;

		public Field(int SizeX, int SizeY)
		{
			this.Tiles = new TileField(SizeX, SizeY);

			this.Container = new Canvas
			{
				Width = Tiles.Width,
				Height = Tiles.Height
			};
			this.Tiles.Container.AttachTo(this.Container);

			this.Pipes = new Canvas
			{
				Width = Tiles.Width,
				Height = Tiles.Height
			}.AttachTo(this.Container);

			this.Tiles.Focus +=
				Selection =>
				{
					if (ShowSelection)
						this[Selection.IndexX, Selection.IndexY].DoIfAny(
							k =>
							{
								k.OverlayBlackAnimationStart();
							}
						);
				};

			this.Tiles.Unfocus +=
				Selection =>
				{
					if (ShowSelection)
						this[Selection.IndexX, Selection.IndexY].DoIfAny(
							k =>
							{
								k.OverlayBlackAnimationStop();
							}
						);
				};
		}

		public bool ShowSelection;

		[Script]
		public class SimplePipeOnTheField
		{
			public int X;
			public int Y;

			public SimplePipe Value;
		}

		public readonly List<SimplePipeOnTheField> PipesList = new List<SimplePipeOnTheField>();

		public SimplePipe this[Tile e]
		{
			get
			{
				return this[e.IndexX, e.IndexY];
			}
			set
			{
				this[e.IndexX, e.IndexY] = value; 
			}
		}

		[Script]
		public class FindSiblings
		{
			public Action<SimplePipeOnTheField> FoundLeft;
			public Action<SimplePipeOnTheField> FoundTop;
			public Action<SimplePipeOnTheField> FoundRight;
			public Action<SimplePipeOnTheField> FoundBottom;

			public void Apply(IEnumerable<SimplePipeOnTheField> source, SimplePipeOnTheField target)
			{
				source.Where(k => k.Y == target.Y).FirstOrDefault(k => k.X == target.X - 1).DoIfAny(
					FoundLeft
				);

				source.Where(k => k.Y == target.Y).FirstOrDefault(k => k.X == target.X + 1).DoIfAny(
					FoundRight
				);

				source.Where(k => k.X == target.X).FirstOrDefault(k => k.Y == target.Y - 1).DoIfAny(
					FoundTop
				);

				source.Where(k => k.X == target.X).FirstOrDefault(k => k.Y == target.Y + 1).DoIfAny(
					FoundBottom 
				);
			}
		}

		public SimplePipe this[int x, int y]
		{
			get
			{
				var v = PipesList.Where(k => k.Y == y).FirstOrDefault(k => k.X == x);

				if (v == null)
					return null;

				return v.Value;
			}
			set
			{
				#region remove old pipe
				PipesList.Where(k => k.Y == y).Where(k => k.X == x).ToArray().ForEach(
					target =>
					{
						Console.WriteLine("remove: " + new { target.Value.GetType().Name, target.X, target.Y });

						target.Value.Container.Orphanize();
						PipesList.Remove(target);

						new FindSiblings
						{
							FoundLeft = Left =>
							{
								Left.Value.Output.Right = null;
								value.Output.Left = null;
							},
							FoundRight = Right =>
							{
								Right.Value.Output.Left = null;
								value.Output.Right = null;
							},
							FoundTop = Top =>
							{
								Top.Value.Output.Bottom = null;
								value.Output.Top = null;
							},
							FoundBottom = Bottom =>
							{
								Bottom.Value.Output.Top = null;
								value.Output.Bottom = null;
							}
						}.Apply(PipesList, target);
					}
				);
				#endregion

				{
					value.PipeParts.ForEach(k => k.Color = this.DefaultPipeColor);

					var target =
						new SimplePipeOnTheField
						{
							Value = value,
							X = x,
							Y = y
						};

					PipesList.Add(
						target
					);

					Console.WriteLine("add: " + new { target.Value.GetType().Name, target.X, target.Y });

					value.Container.AttachTo(this.Pipes).MoveTo(
						x * Tile.Size + Tile.ShadowBorder,
						(1 + y) * Tile.SurfaceHeight + Tile.ShadowBorder - Tile.Size
					);

					new FindSiblings
					{
						FoundLeft = Left =>
						{
							Left.Value.Output.Right = value.Input.Left;
							value.Output.Left = Left.Value.Input.Right;
						},
						FoundRight = Right =>
						{
							Right.Value.Output.Left = value.Input.Right;
							value.Output.Right = Right.Value.Input.Left;
						},
						FoundTop = Top =>
						{
							Top.Value.Output.Bottom = value.Input.Top;
							value.Output.Top = Top.Value.Input.Bottom;
						},
						FoundBottom = Bottom =>
						{
							Bottom.Value.Output.Top = value.Input.Bottom;
							value.Output.Bottom = Bottom.Value.Input.Top;
						}
					}.Apply(PipesList, target);

				}




			}
		}

		public void RefreshPipes()
		{
			foreach (var k in this.PipesList)
			{
				k.Value.Container.Orphanize();
			}


			foreach (var k in this.PipesList.OrderBy(k => k.Y))
			{
				k.Value.Container.AttachTo(this.Pipes);
			}
		}
	}
}
