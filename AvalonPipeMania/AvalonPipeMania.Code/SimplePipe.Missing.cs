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
		public class Missing : SimplePipe
		{
			readonly Pipe.Missing PipeMissing;

			// this pipe should be added to the field on demand

			public Missing()
			{
				this.IsVirtualPipe = true;

				this.PipeMissing = new Pipe.Missing();

				this.PipeMissing.WaterDropFromLeftAnimation.Stop();
				this.PipeMissing.WaterDropFromRightAnimation.Stop();
				this.PipeMissing.WaterDropFromTopAnimation.Stop();
				this.PipeMissing.WaterDropFromBottomAnimation.Stop();

				this.PipeMissing.Container.AttachTo(this.Container);


				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Left =
					delegate
					{
						this.PipeMissing.WaterDropFromLeftAnimation.Start();
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};

				this.Input.Right =
					delegate
					{
						this.PipeMissing.WaterDropFromRightAnimation.Start();
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};

				this.Input.Top =
					delegate
					{
						this.PipeMissing.WaterDropFromTopAnimation.Start();
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};


				this.Input.Bottom =
					delegate
					{
						this.PipeMissing.WaterDropFromBottomAnimation.Start();
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};

				this.PipeParts = new Pipe[]
				{
					this.PipeMissing
				};
			}
		}
	}
}
