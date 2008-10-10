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
		public class LeftToDrain : SimplePipe
		{
			readonly Pipe.LeftToDrain PipeLeftToDrain;


			public LeftToDrain()
			{
				this.PipeLeftToDrain = new Pipe.LeftToDrain();
				this.PipeLeftToDrain.WaterDropAnimation.Stop();

				this.PipeLeftToDrain.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Left =
					delegate
					{
						Action Output = null;

						Output += this.Output.Drain;
						Output += this.PipeLeftToDrain.WaterDropAnimation.Start;

						Animate(this.PipeLeftToDrain.Water, Output);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipeLeftToDrain
				};

			}
		}
	}
}
