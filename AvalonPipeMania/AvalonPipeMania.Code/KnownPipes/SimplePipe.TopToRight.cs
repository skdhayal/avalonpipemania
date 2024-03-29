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
		public class TopToRight : SimplePipe
		{
			readonly Pipe PipeTopToRight;


			public TopToRight()
			{
				this.PipeTopToRight = new Pipe.TopToRight();

				this.PipeTopToRight.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.SupportedOutput.Right = SupportedOutputMarker;
				this.Input.Top =
					delegate
					{
						Animate(this.PipeTopToRight.Water, this.Output.RaiseRight);
					};

				this.SupportedOutput.Top = SupportedOutputMarker;
				this.Input.Right =
					delegate
					{
						Animate(this.PipeTopToRight.Water.Reverse(), this.Output.RaiseTop);
					};

			
				this.PipeParts = new Pipe[]
				{
					this.PipeTopToRight
				};
			}
		}
	}
}
