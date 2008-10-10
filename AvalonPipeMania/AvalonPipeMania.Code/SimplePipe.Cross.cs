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
		public class Cross : SimplePipe
		{
			readonly Pipe.TopToBottom PipeTopToBottom;

			readonly Pipe.LeftToRightBent PipeLeftToRight;


			public Cross()
			{
				
				#region vertical
				this.PipeTopToBottom = new Pipe.TopToBottom();

				this.PipeTopToBottom.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Top =
					delegate
					{
						Animate(this.PipeTopToBottom.Water, this.Output.Bottom);
					};

				this.Input.Bottom =
					delegate
					{
						Animate(this.PipeTopToBottom.Water.Reverse(), this.Output.Top);
					};
				#endregion

				#region horizontal
				this.PipeLeftToRight = new Pipe.LeftToRightBent();

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
				#endregion


				this.PipeParts = new Pipe[]
				{
					this.PipeTopToBottom,
					this.PipeLeftToRight
				};
		


			}
		}
	}
}
