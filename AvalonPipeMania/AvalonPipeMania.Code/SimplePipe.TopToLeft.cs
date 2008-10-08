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
		public class TopToLeft : SimplePipe
		{
			readonly Pipe PipeTopToLeft;


			public TopToLeft()
			{
				this.PipeTopToLeft = new Pipe.TopToLeft();

				this.PipeTopToLeft.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Top =
					delegate
					{
						Animate(this.PipeTopToLeft.Water, this.Output.Left);
					};

				this.Input.Left =
					delegate
					{
						Animate(this.PipeTopToLeft.Water.Reverse(), this.Output.Top);
					};

				this.OverlayBlackAnimationStartEvent += this.PipeTopToLeft.OverlayBlackAnimationStart;
				this.OverlayBlackAnimationStopEvent += this.PipeTopToLeft.OverlayBlackAnimationStop;

			}
		}
	}
}
