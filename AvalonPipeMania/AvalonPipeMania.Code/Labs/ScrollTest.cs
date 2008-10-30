using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class ScrollTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public ScrollTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			this.ClipTo(0, 0, DefaultWidth, DefaultHeight);

			var f = new Field(16, 16);

			// f.Tiles.Color = Colors.Pink;
			f.Tiles.Color = Colors.Cyan;

			#region random pumps and drain holes
			var Randomized = f.Tiles.TileList.Randomize().ToArray();

			Enumerable.Range(0, f.Tiles.SizeX * f.Tiles.SizeY / 20).ForEach(
				Index =>
				{
					var Target = Randomized[Index];
					if (Index % 2 == 0)
					{
						Target.Drain.Show();

						if (Index % 4 == 0)
							f[Target] = new SimplePipe.LeftToDrain();
						else
							f[Target] = new SimplePipe.RightToDrain();

					}
					else
					{
						if (Index % 4 == 1)
							f[Target] = new SimplePipe.PumpToLeft();
						else
							f[Target] = new SimplePipe.PumpToRight();
						10000.AtDelay(f[Target].Input.Pump);
					}
				}
			);
			#endregion

			f.Container.AttachTo(this);

			#region overlay
			var Overlay = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);

			var OverlayRectangle = new Rectangle
			{
				Fill = Brushes.Black,
				Width = DefaultWidth,
				Height = DefaultHeight,
				Opacity = 0
			}.AttachTo(Overlay);



			f.Tiles.Overlay.AttachTo(Overlay);
			#endregion

			#region move the map with the mouse yet not too often anf smooth enough
			Action<int, int> MoveTo = NumericEmitter.Of(
				(x, y) =>
				{
					f.Tiles.Overlay.MoveTo(x, y);
					f.Container.MoveTo(x, y);
				}
			);

			Action<int, int> CalculateMoveTo =
				//Tween.NumericOmitter.Of(
				(int_x, int_y) =>
				{
					double x = int_x;
					double y = int_y;

					//Console.WriteLine(new { x, y }.ToString());

					const int PaddingX = Tile.Size / 2;
					const int PaddingY = Tile.SurfaceHeight;

					const int MarginLeft = Tile.ShadowBorder + Tile.Size + PaddingX;
					const int MarginWidth = MarginLeft + Tile.ShadowBorder + Tile.Size + PaddingX;

					const int MarginTop = Tile.ShadowBorder + Tile.SurfaceHeight + PaddingY;
					const int MarginHeight = MarginTop + Tile.ShadowBorder + Tile.Size + PaddingY;

					var _x = PaddingX + (DefaultWidth - (f.Tiles.Width + PaddingX * 2)) * ((x - MarginLeft).Max(0) / (DefaultWidth - MarginWidth)).Min(1);
					var _y = PaddingY + (DefaultHeight - (f.Tiles.Height + PaddingY * 2)) * ((y - MarginTop).Max(0) / (DefaultHeight - MarginHeight)).Min(1);


					MoveTo(Convert.ToInt32(_x), Convert.ToInt32(_y));
				}
				//)
			;
			#endregion


			Overlay.MouseMove +=
				(Sender, Arguments) =>
				{
					var p = Arguments.GetPosition(Overlay);

					CalculateMoveTo(Convert.ToInt32(p.X), Convert.ToInt32(p.Y));
				};

			f.Tiles.Click +=
				Selected =>
				{
					if (Selected == null)
						return;

					f[Selected] = null;

					Selected.Hide();
				};
		}
	}
}
