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
		public class RightToDrain : SimplePipe
		{
			readonly Pipe.RightToDrain PipeRightToDrain;


			public RightToDrain()
			{
				this.PipeRightToDrain = new Pipe.RightToDrain();
				this.PipeRightToDrain.WaterDropAnimation.Stop();

				this.PipeRightToDrain.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Right =
					delegate
					{
						Action Output = null;

						Output += this.Output.Drain;
						Output += this.PipeRightToDrain.WaterDropAnimation.Start;

						Animate(this.PipeRightToDrain.Water, Output);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipeRightToDrain
				};

			}
		}
	}
}
