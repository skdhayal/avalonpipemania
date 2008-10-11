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
using AvalonPipeMania.Code.Tween;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class ExtendedFieldTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public ExtendedFieldTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;


			var f = new ExtendedField(6, 12, DefaultWidth, DefaultHeight);

			f.Container.AttachTo(this);

			

			
			var feeder = new SimplePipeFeeder(6);

			feeder.Container.MoveTo(Tile.ShadowBorder, Tile.ShadowBorder + Tile.Size).AttachTo(this);

			f.Overlay.AttachTo(this);

			f.Field.Tiles.Click +=
				delegate
				{
					feeder.MoveNext();

					f.PipeToBeBuilt = feeder.Current;
				};

			f.PipeToBeBuilt = feeder.Current;

		}

	
	
	}
}
