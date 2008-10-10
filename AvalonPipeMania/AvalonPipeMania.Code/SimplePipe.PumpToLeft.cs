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
		public class PumpToLeft : SimplePipe
		{
			readonly Pipe.PumpToLeft PipePumpToLeft;


			public PumpToLeft()
			{
				this.PipePumpToLeft = new Pipe.PumpToLeft();
				this.PipePumpToLeft.PumpHandleAnimation.Stop();

				this.PipePumpToLeft.Container.AttachTo(this.Container);

				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Pump =
					delegate
					{
						this.PipePumpToLeft.PumpHandleAnimation.Start();

						// turn the handle before the water comes
						1500.AtDelay(
							delegate
							{
								1500.AtDelay(this.PipePumpToLeft.PumpHandleAnimation.Stop);

								Animate(this.PipePumpToLeft.Water, this.Output.Left);
							}
						);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipePumpToLeft
				};

			}
		}
	}
}
