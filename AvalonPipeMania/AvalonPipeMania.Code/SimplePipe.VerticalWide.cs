﻿using System;
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
		public class VerticalWide : SimplePipe
		{
			readonly Pipe.TopToBottomWide PipeTopToBottom;

			public VerticalWide()
			{
				this.WaterAnimationSpeed *= 10;

				this.PipeTopToBottom = new Pipe.TopToBottomWide();

				this.PipeTopToBottom.Container.AttachTo(this.Container);



				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.SupportedOutput.Bottom = SupportedOutputMarker;
				this.Input.Top =
					delegate
					{
						this.AnimateTopToBottom(
							this.PipeTopToBottom.Water.First(),
							this.Output.RaiseBottom
						);
					};

				this.SupportedOutput.Top = SupportedOutputMarker;
				this.Input.Bottom =
					delegate
					{
						var water = this.PipeTopToBottom.Water.Last();

						this.AnimateBottomToTop(
							this.PipeTopToBottom.Water.Last(),
							this.Output.RaiseTop
						);

				
					};


				this.PipeParts = new Pipe[]
				{
					this.PipeTopToBottom
				};
			}
		}
	}
}
