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

					const int MarginX = Tile.ShadowBorder + Tile.Size;
					const int MarginY = Tile.ShadowBorder + Tile.SurfaceHeight;
					
					var x = (DefaultWidth - f.Tiles.Width) * ((p.X - MarginX).Max(0) / (DefaultWidth - MarginX * 2)).Min(1);
					var y = (DefaultHeight - f.Tiles.Height) * ((p.Y - MarginY).Max(0) / (DefaultHeight - MarginY * 2)).Min(1);

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
