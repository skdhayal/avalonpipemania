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
		public class Vertical : SimplePipe
		{
			readonly Pipe.TopToBottom PipeTopToBottom;

			public Vertical()
			{
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


				this.PipeParts = new Pipe[]
				{
					this.PipeTopToBottom
				};
			}
		}
	}
}
