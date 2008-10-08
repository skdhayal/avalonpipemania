using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;


namespace AvalonPipeMania.Code
{
	[Script]
	public class Field
	{
		public readonly Canvas Container;

		public readonly Canvas Pipes;

		public readonly TileField Tiles;

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


		}

		[Script]
		public class SimplePipeOnTheField
		{
			public int X;
			public int Y;

			public SimplePipe Value;
		}

		public readonly List<SimplePipeOnTheField> PipesList = new List<SimplePipeOnTheField>();

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
				PipesList.Add(
					new SimplePipeOnTheField
					{
						Value = value,
						X = x,
						Y = y
					}
				);

				value.Container.AttachTo(this.Pipes).MoveTo(
					x * Tile.Size + Tile.ShadowBorder,
					(1 + y) * Tile.SurfaceHeight + Tile.ShadowBorder - Tile.Size
				);

				PipesList.Where(k => k.Y == y).FirstOrDefault(k => k.X == x - 1).DoIfAny(
					Left =>
					{
						Left.Value.Output.Right = value.Input.Left;
						value.Output.Left = Left.Value.Input.Right;
					}
				);

				PipesList.Where(k => k.Y == y).FirstOrDefault(k => k.X == x + 1).DoIfAny(
					Right =>
					{
						Right.Value.Output.Left = value.Input.Right;
						value.Output.Right = Right.Value.Input.Left;
					}
				);

				PipesList.Where(k => k.X == x).FirstOrDefault(k => k.Y == y - 1).DoIfAny(
					Top =>
					{
						Top.Value.Output.Bottom = value.Input.Top;
						value.Output.Top = Top.Value.Input.Bottom;
					}
				);

				PipesList.Where(k => k.X == x).FirstOrDefault(k => k.Y == y + 1).DoIfAny(
					Bottom =>
					{
						Bottom.Value.Output.Top = value.Input.Bottom;
						value.Output.Bottom = Bottom.Value.Input.Top;
					}
				);

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
