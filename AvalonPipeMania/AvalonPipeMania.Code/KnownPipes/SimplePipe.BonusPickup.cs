using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonPipeMania.Code.Extensions;
using AvalonPipeMania.Code.Assets;

namespace AvalonPipeMania.Code
{
	partial class SimplePipe
	{

		[Script]
		public class BonusPickup : SimplePipe
		{
			// this pipe should be added to the field on demand

			AnimationCollection.Animation ChosenAnimation;

			public BonusPickup()
			{
				this.DemolitionCost = 400;

				this.IsVirtualPipe = true;

				this.ChosenAnimation = KnownAnimations.Emoticons.Random().ToAnimation().AttachContainerTo(this.Container);

				this.ChosenAnimation.MoveContainerTo(
					(Pipe.Size - this.ChosenAnimation.Info.Width) / 2,
					(Pipe.Size - this.ChosenAnimation.Info.Height) / 2
				);

				this.Disposing +=
					delegate
					{
						this.ChosenAnimation.Dispose();
					};
				
				this.PipeParts = new Pipe[]
				{
				};
			}
		}
	}
}
