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

			Overlay.MouseMove +=
				(Sender, Arguments) =>
				{
					var p = Arguments.GetPosition(Overlay);

					const int PaddingX = Tile.Size / 2;
					const int PaddingY = Tile.SurfaceHeight / 2;

					const int MarginLeft = Tile.ShadowBorder + Tile.Size + PaddingX;
					const int MarginWidth = MarginLeft + Tile.ShadowBorder + Tile.Size + PaddingX;

					const int MarginTop = Tile.ShadowBorder + Tile.SurfaceHeight + PaddingY;
					const int MarginHeight = MarginTop + Tile.ShadowBorder + Tile.Size + PaddingY;

					var x = PaddingX + (DefaultWidth - (f.Tiles.Width + PaddingX * 2)) * ((p.X - MarginLeft).Max(0) / (DefaultWidth - MarginWidth)).Min(1);
					var y = PaddingY + (DefaultHeight - (f.Tiles.Height + PaddingY * 2)) * ((p.Y - MarginTop).Max(0) / (DefaultHeight - MarginHeight)).Min(1);

					f.Tiles.Overlay.MoveTo(x, y);
					f.Container.MoveTo(x, y);
				};

			f.Tiles.Click +=
				Selected =>
				{
					Selected.Hide();
				};
		}
	}
}
