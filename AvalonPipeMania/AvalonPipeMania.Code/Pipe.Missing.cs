using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvalonPipeMania.Assets.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Threading;

namespace AvalonPipeMania.Code
{
	partial class Pipe
	{
	

		[Script]
		public class Missing : Pipe
		{
			public readonly DispatcherTimer WaterDropFromLeftAnimation;
			public readonly DispatcherTimer WaterDropFromRightAnimation;


			public Missing()
			{
				var f = new Factory(KnownAssets.Path.Pipe.Missing, this.Container);

				this.Water = f.ToWaterImages(
					"1",
					"2",
					"3"
				);

				{
					var WaterDropFrames = f.ToWaterImages(
						"l1",
						"l2"
					);

					Action Hide = delegate { };

					WaterDropFromLeftAnimation = (1000 / 23).AtIntervalWithCounter(
						Counter =>
						{
							Hide();

							WaterDropFrames.AtModulus(Counter).Visibility = Visibility.Visible;

							Hide = () => WaterDropFrames.AtModulus(Counter).Visibility = Visibility.Hidden;
						}
					);
				}

				{
					var WaterDropFrames = f.ToWaterImages(
						"r1",
						"r2"
					);

					Action Hide = delegate { };

					WaterDropFromRightAnimation = (1000 / 23).AtIntervalWithCounter(
						Counter =>
						{
							Hide();

							WaterDropFrames.AtModulus(Counter).Visibility = Visibility.Visible;

							Hide = () => WaterDropFrames.AtModulus(Counter).Visibility = Visibility.Hidden;
						}
					);
				}


			}
		}

		
	}


}
