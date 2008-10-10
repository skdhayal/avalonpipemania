using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Diagnostics;

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

		public IEnumerable<SimplePipeOnTheField> ByIndex(int x, int y)
		{
			return PipesList.Where(k => k.Y == y).Where(k => k.X == x);
		}

		public SimplePipe this[int x, int y]
		{
			get
			{
				var v = ByIndex(x, y).FirstOrDefault(k => !k.Value.IsVirtualPipe);

				if (v == null)
					return null;

				return v.Value;
			}
			set
			{
				// assigning null currently does not register spill detection

				var IsVirtualPipe = false;

				if (value != null)
					IsVirtualPipe = value.IsVirtualPipe;

				#region Spill
				Action<int, int, SimplePipeOnTheField> Spill =
					(ox, oy, SpillTarget) =>
					{
						if (SpillTarget.Value.Output[ox, oy] == null)
							if (SpillTarget.Value.SupportedOutput[ox, oy] != null)
							{
								// once the water gets here we need to spill it on the floor
								// it can happen when the pipe is not there or even when the pipe does not accept input


								// when the correct pipe is built this event is effectevly detatched
								SpillTarget.Value.Output[ox, oy] =
									delegate
									{
										// SpillTarget has filled itself with water
										// and needs to continue on the next pipe
										// but as we are inside here there is no matching pipe there

										var s = new SimplePipe.Missing();

										this[SpillTarget.X + ox, SpillTarget.Y + oy] = s;

										RefreshPipes();

										s.Output.Spill =
											delegate
											{
												// game over!!!
											};

										// we need to trigger the event
										s.Input[-ox, -oy]();

										// and when the water hits the floor
										// we should trigger end game
									};
							}
					};
				#endregion


				if (!IsVirtualPipe)
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

									if (value != null)
										value.Output.Left = null;

									Spill(1, 0, Left);
								},
								FoundRight = Right =>
								{
									Right.Value.Output.Left = null;
									if (value != null)
										value.Output.Right = null;

									Spill(-1, 0, Right);
								},
								FoundTop = Top =>
								{
									Top.Value.Output.Bottom = null;
									if (value != null)
										value.Output.Top = null;

									Spill(0, 1, Top);
								},
								FoundBottom = Bottom =>
								{
									Bottom.Value.Output.Top = null;
									if (value != null)
										value.Output.Bottom = null;

									Spill(0, -1, Bottom);
								}
							}.Apply(PipesList, target);
						}
					);
					#endregion
				}

				if (value != null)
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

					if (!IsVirtualPipe)
					{
					
						new FindSiblings
						{
							FoundLeft = Left =>
							{
								Left.Value.Output.Right = value.Input.Left;
								value.Output.Left = Left.Value.Input.Right;

								Spill(1, 0, Left);
							},
							FoundRight = Right =>
							{
								Right.Value.Output.Left = value.Input.Right;
								value.Output.Right = Right.Value.Input.Left;

								Spill(-1, 0, Right);
							},
							FoundTop = Top =>
							{
								Top.Value.Output.Bottom = value.Input.Top;
								value.Output.Top = Top.Value.Input.Bottom;

								Spill(0, 1, Top);
							},
							FoundBottom = Bottom =>
							{
								Bottom.Value.Output.Top = value.Input.Bottom;
								value.Output.Bottom = Bottom.Value.Input.Top;

								Spill(0, -1, Bottom);
							}
						}.Apply(PipesList, target);

						Spill(1, 0, target);
						Spill(-1, 0, target);
						Spill(0, 1, target);
						Spill(0, -1, target);

					}
				}




			}
		}

		public void RefreshPipes()
		{
			foreach (var k in this.PipesList)
			{
				k.Value.Container.Orphanize();
			}


			foreach (var k in this.PipesList.OrderBy(k => k.Y).ThenBy(k => !k.Value.IsVirtualPipe))
			{
				k.Value.Container.AttachTo(this.Pipes);
			}
		}
	}
}
