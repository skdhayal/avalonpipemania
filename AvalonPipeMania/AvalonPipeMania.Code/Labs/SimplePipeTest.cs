using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using ScriptCoreLib;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	class SimplePipeTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public SimplePipeTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			var x = 200;
			var y = 200;

		

			var h4 = new SimplePipe.PumpToLeft();
			h4.Container.MoveTo(x + Pipe.Size, y);

			var h = new SimplePipe.Horizontal();
			h4.Output.Left = h.Input.Right;
			h.Container.MoveTo(x, y);

			var h2 = new SimplePipe.Horizontal();
			h2.Container.MoveTo(x - Pipe.Size, y);
			h.Output.Left = h2.Input.Right;

			var h3 = new SimplePipe.TopToRight();
			h3.Container.MoveTo(x - Pipe.Size * 2, y);
			h2.Output.Left = h3.Input.Right;

			var h5 = new SimplePipe.Vertical();
			h5.Container.MoveTo(x - Pipe.Size * 2, y - Tile.SurfaceHeight);
			h3.Output.Top = h5.Input.Bottom;

			var h6 = new SimplePipe.Vertical();
			h6.Container.MoveTo(x - Pipe.Size * 2, y - Tile.SurfaceHeight * 2);
			h5.Output.Top = h6.Input.Bottom;

			var h7 = new SimplePipe.RightToBottom();
			h7.Container.MoveTo(x - Pipe.Size * 2, y - Tile.SurfaceHeight * 3);
			h6.Output.Top = h7.Input.Bottom;

			var h8 = new SimplePipe.Horizontal();
			h8.Container.MoveTo(x - Pipe.Size * 1, y - Tile.SurfaceHeight * 3);
			h7.Output.Right = h8.Input.Left;


			5000.AtDelay(h4.Input.Pump);

			new Canvas[]
			{
				h8.Container,
				h7.Container, 
				h6.Container,
				h5.Container,
				h4.Container,
				h3.Container,
				h2.Container,
				h.Container
			}.AttachTo(this);
		}
	}
}
