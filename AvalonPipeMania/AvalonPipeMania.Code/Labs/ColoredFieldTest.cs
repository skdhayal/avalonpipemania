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

		

		}


		public enum FeederState
		{
			LeftAndIdle,
			LeftAndMoving,
			RightAndIdle,
			RightAndMoving
		}
	}
}
