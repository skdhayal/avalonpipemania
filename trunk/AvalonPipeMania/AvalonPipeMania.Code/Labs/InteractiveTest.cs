using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class InteractiveTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;


		public InteractiveTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			#region background
			new[]
			{
				//Colors.White,
				//Colors.Blue,
				Colors.Red,
				Colors.Yellow
			}.ToGradient(DefaultHeight / 4).ForEach(
				(c, i) =>
				new Rectangle
				{
					Fill = new SolidColorBrush(c),
					Width = DefaultWidth,
					Height = 5,
				}.MoveTo(0, i * 4).AttachTo(this)
			);
			#endregion


			// what do we need to do:
			// show all possible pipe buttons
			// make them selectable and placeable

			const int PipeSizeWithMargin = Pipe.Size + 8;

			Pipe.KnownPipes.ForEach(
				(pipe, i) =>
				{
					pipe.Container.AttachTo(this).MoveTo(32 + (i % 2) * PipeSizeWithMargin, 32 + PipeSizeWithMargin * i / 2);
				}
			);
		}
	}
}
