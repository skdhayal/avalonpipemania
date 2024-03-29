﻿using System;
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

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class ColoredFieldTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public ColoredFieldTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;


			var f = new ExtendedField(16, 6, DefaultWidth, DefaultHeight);

			f.Field.DefaultPipeColor = Colors.Green;

			f.Field.Tiles.Color = Colors.Pink;

			f.Container.AttachTo(this);

			#region feeder
			var feeder = new SimplePipeFeeder(6, Colors.Yellow);

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
			var Randomized = f.Field.Tiles.TileList.Randomize().GetEnumerator();

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
