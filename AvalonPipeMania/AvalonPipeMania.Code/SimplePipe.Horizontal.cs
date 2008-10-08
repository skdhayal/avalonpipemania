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
		public class Horizontal : SimplePipe
		{
			readonly Pipe.LeftToRight PipeLeftToRight;

			public Horizontal()
			{
				this.PipeLeftToRight = new Pipe.LeftToRight();

				this.PipeLeftToRight.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.OverlayBlackAnimationStart += this.PipeLeftToRight.OverlayBlackAnimationStart;
				this.OverlayBlackAnimationStop += this.PipeLeftToRight.OverlayBlackAnimationStop;

				this.Input.Left =
					delegate
					{
						Animate(this.PipeLeftToRight.Water, this.Output.Right);
					};

				this.Input.Right =
					delegate
					{
						Animate(this.PipeLeftToRight.Water.Reverse(), this.Output.Left);
					};
			}
		}
	}
}
