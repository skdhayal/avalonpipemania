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
		public class LeftToBottom : SimplePipe
		{
			readonly Pipe PipeLeftToBottom;


			public LeftToBottom()
			{
				this.PipeLeftToBottom = new Pipe.LeftToBottom();

				this.PipeLeftToBottom.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Left =
					delegate
					{
						Animate(this.PipeLeftToBottom.Water, this.Output.Bottom);
					};

				this.Input.Bottom =
					delegate
					{
						Animate(this.PipeLeftToBottom.Water.Reverse(), this.Output.Left);
					};

				this.OverlayBlackAnimationStartEvent += this.PipeLeftToBottom.OverlayBlackAnimationStart;
				this.OverlayBlackAnimationStopEvent += this.PipeLeftToBottom.OverlayBlackAnimationStop;

			}
		}
	}
}
