using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonPipeMania.Code
{
	partial class SimplePipe
	{

		[Script]
		public class HorizontalWide : SimplePipe
		{
			readonly Pipe.LeftToRightWide PipeLeftToRight;

			public HorizontalWide()
			{
				WaterAnimationSpeed *= 4;

				this.PipeLeftToRight = new Pipe.LeftToRightWide();

				this.PipeLeftToRight.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.


				this.SupportedOutput.Right = SupportedOutputMarker;
				this.Input.Left =
					delegate
					{
						Animate(this.PipeLeftToRight.Water, this.Output.Right);
					};

				this.SupportedOutput.Left = SupportedOutputMarker;
				this.Input.Right =
					delegate
					{
						Animate(this.PipeLeftToRight.Water.Reverse(), this.Output.Left);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipeLeftToRight
				};
			}
		}
	}
}
