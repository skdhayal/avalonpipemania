using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class FieldTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public FieldTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			var f = new Field(6, 4);

			#region round the corners
			f.Tiles[0, 0].Hide();
			f.Tiles[f.Tiles.SizeX - 1, 0].Hide();
			f.Tiles[f.Tiles.SizeX - 1, f.Tiles.SizeY - 1].Hide();
			f.Tiles[0, f.Tiles.SizeY - 1].Hide();
			#endregion

			f[2, 0] = new SimplePipe.PumpToLeft();

			f[1, 0] = new SimplePipe.RightToBottom();
			f[1, 1] = new SimplePipe.Vertical();
			f[1, 2] = new SimplePipe.TopToRight();
			f[2, 2] = new SimplePipe.Cross();
			f[3, 2] = new SimplePipe.LeftToDrain();

			f[2, 1] = new SimplePipe.RightToBottom();
			f[2, 3] = new SimplePipe.TopToRight();
			f[3, 1] = new SimplePipe.Horizontal();
			f[3, 3] = new SimplePipe.Horizontal();

			// show a hole in the floor
			f.Tiles[3, 2].Drain.Visibility = System.Windows.Visibility.Visible;

			// user must click on the pump to activate it
			f.Tiles[2, 0].Overlay.MouseLeftButtonUp +=
				delegate
				{
					f[2, 0].Input.Pump();
				};

			f.RefreshPipes();

			var x = (DefaultWidth - f.Tiles.Width) / 2;
			var y = (DefaultHeight - f.Tiles.Height) / 2;

			f.Container.MoveTo(x, y).AttachTo(this);
			f.Tiles.Overlay.MoveTo(x, y).AttachTo(this);
		}
	}
}
