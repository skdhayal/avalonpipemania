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
using AvalonPipeMania.Code.Extensions;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class SpaceInvaderTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public SpaceInvaderTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			// SpaceInvaderTest
			// http://www.glassgiant.com/ascii/
			// http://text-image.com/convert/
			// these maps could be inside a zip file

			var map = new ASCIIImage(@"
MMMMMMM...MMMMMMMMMMMMMMMMMMM...MMMMMMMM
MMMMMMM...MMMMMMMMMMMMMMMMMMM...MMMMMMMM
MMMMMMMMMM    MMMMMMMMMMM    MMMMMMMMMMM
MMMMMMMMMM    MMMMMMMMMMM    MMMMMMMMMMM
MMMMMMM                         MMMMMMMM
MMMMMMM                         MMMMMMMM
MMM       MMMM           MMMM       MMMM
MMM       MMMM           MMMM       MMMM
MMM       MMMM           MMMM       MMMM
...                                     
...                                     
...                                     
...                                     
...MMMM   MMMMMMMMMMMMMMMMMMM   MMMM    
...MMMM   MMMMMMMMMMMMMMMMMMM   MMMM    
MMMMMMMMMM        MMM.       MMMMMMMMMMM
MMMMMMMMMM        MMM.       MMMMMMMMMMM
MMMMMMMMMM        MMM.       MMMMMMMMMMM
			".Trim());



			var f = new ExtendedField(map.Width, map.Height, DefaultWidth, DefaultHeight);

			for (int x = 0; x < map.Width; x++)
				for (int y = 0; y < map.Height; y++)
				{
					if (map[x, y] == "M")
						f.Field.Tiles[x, y].Hide();
				}

			f.Field.DefaultPipeColor = Colors.Yellow;

			f.Field.Tiles.Color = Colors.Cyan;

			f.Container.AttachTo(this);

			#region feeder
			var feeder = new SimplePipeFeeder(6, Colors.Brown);

			feeder.Container.AttachTo(this);

			var feeder_autohide = new SimplePipeFeeder.AutoHide(feeder, f.Overlay, DefaultWidth, DefaultHeight);



			f.Overlay.AttachTo(this);

			f.PipeToBeBuilt = feeder.Current;
			f.PipeToBeBuiltUsed +=
				delegate
				{
					feeder.MoveNext();

					f.PipeToBeBuilt = feeder.Current;
				};
			#endregion



			#region setting up some stuff on the field
			var Randomized = f.Field.Tiles.TileList.Where(k => k.IsVisible).Randomize().GetEnumerator();

			Enumerable.Range(0, 4).ForEach(
				Index =>
				{
					var Target = Randomized.Take();

					Target.Drain.Show();

					if (Index % 2 == 0)
						f.Field[Target] = new SimplePipe.LeftToDrain();
					else
						f.Field[Target] = new SimplePipe.RightToDrain();
				}
			);

			var PumpTimeout = new Random();

			Enumerable.Range(0, 4).ForEach(
				Index =>
				{
					var Target = Randomized.Take();

					var pump = default(SimplePipe);

					if (Index % 2 == 0)
						pump = new SimplePipe.PumpToLeft();
					else
						pump = new SimplePipe.PumpToRight();

					pump.AddTimer(PumpTimeout.Next(20, 150), pump.Input.Pump);

					f.Field[Target] = pump;
				}
			);
			#endregion


		}


	}
}
