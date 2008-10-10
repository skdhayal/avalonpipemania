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
		public class PumpToRight : SimplePipe
		{
			readonly Pipe.PumpToRight PipePumpToRight;


			public PumpToRight()
			{
				this.PipePumpToRight = new Pipe.PumpToRight();
				this.PipePumpToRight.PumpHandleAnimation.Stop();

				this.PipePumpToRight.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Pump =
					delegate
					{
						this.PipePumpToRight.PumpHandleAnimation.Start();

						// turn the handle before the water comes
						1500.AtDelay(
							delegate
							{
								1500.AtDelay(this.PipePumpToRight.PumpHandleAnimation.Stop);

								Animate(this.PipePumpToRight.Water, this.Output.Right);
							}
						);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipePumpToRight
				};

			}
		}
	}
}
