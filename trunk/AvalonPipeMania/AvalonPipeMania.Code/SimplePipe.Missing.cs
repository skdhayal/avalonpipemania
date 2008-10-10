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
				this.PipeMissing = new Pipe.Missing();

				this.PipeMissing.Container.AttachTo(this.Container);


				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.Input.Left =
					delegate
					{
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};

				this.Input.Right =
					delegate
					{
						Animate(this.PipeMissing.Water, this.Output.Spill);
					};


				this.PipeParts = new Pipe[]
				{
				};
			}
		}
	}
}
