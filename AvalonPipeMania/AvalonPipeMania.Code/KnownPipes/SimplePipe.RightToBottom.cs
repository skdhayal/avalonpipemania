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
		public class RightToBottom : SimplePipe
		{
			readonly Pipe PipeRightToBottom;


			public RightToBottom()
			{
				this.PipeRightToBottom = new Pipe.RightToBottom();

				this.PipeRightToBottom.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.SupportedOutput.Bottom = SupportedOutputMarker;
				this.Input.Right =
					delegate
					{
						Animate(this.PipeRightToBottom.Water, this.Output.RaiseBottom);
					};

				this.SupportedOutput.Right = SupportedOutputMarker;
				this.Input.Bottom =
					delegate
					{
						Animate(this.PipeRightToBottom.Water.Reverse(), this.Output.RaiseRight);
					};

			
				this.PipeParts = new Pipe[]
				{
					this.PipeRightToBottom
				};
			}
		}
	}
}
