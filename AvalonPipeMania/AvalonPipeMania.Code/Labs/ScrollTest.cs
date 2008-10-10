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


			var f = new Field(16, 16);

			f.Tiles.Color = Colors.Pink;

			f.Container.AttachTo(this);

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

			#region move the map with the mouse yet not too often anf smooth enough
			Action<int, int> MoveTo = Tween.NumericEmitter.Of(
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
					Selected.Hide();
				};
		}
	}
}
