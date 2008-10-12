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

				this.SupportedOutput.Bottom = SupportedOutputMarker;
				this.Input.Top =
					delegate
					{
						AnimateTopToBottom(this.PipeTopToBottom.Water.First(), this.Output.RaiseBottom);
					};

				this.SupportedOutput.Top = SupportedOutputMarker;
				this.Input.Bottom =
					delegate
					{
						AnimateBottomToTop(this.PipeTopToBottom.Water.Last(), this.Output.RaiseTop);
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
						AnimateLeftToRight(this.PipeLeftToRight.Water.First(), this.Output.RaiseRight);
					};

				this.SupportedOutput.Left = SupportedOutputMarker;
				this.Input.Right =
					delegate
					{
						AnimateRightToLeft(this.PipeLeftToRight.Water.Last(), this.Output.RaiseLeft);
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
