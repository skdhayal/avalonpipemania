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
			public readonly DispatcherTimer WaterDropFromTopAnimation;
			public readonly DispatcherTimer WaterDropFromBottomAnimation;


			public Missing()
			{
				var f = new Factory(KnownAssets.Path.Pipe.Missing, this.Container);

				this.Water = f.ToWaterImages(
					"1",
					"2",
					"3"
				);

				ParamsFunc<string, DispatcherTimer> ToAnimation =
					Frames =>
					{
						var WaterDropFrames = f.ToWaterImages(
							Frames
						);

						Action Hide = delegate { };

						return (1000 / 23).AtIntervalWithCounter(
							Counter =>
							{
								Hide();

								WaterDropFrames.AtModulus(Counter).Show();

								Hide = WaterDropFrames.AtModulus(Counter).Hide;
							}
						);
					};

				WaterDropFromLeftAnimation = 
					ToAnimation(
						"l1",
						"l2"
					);


				WaterDropFromRightAnimation =
					ToAnimation(
						"r1",
						"r2"
					);


				WaterDropFromTopAnimation =
					ToAnimation(
						"t1",
						"t2"
					);

				WaterDropFromBottomAnimation =
					ToAnimation(
						"b1",
						"b2"
					);


			}
		}

		
	}


}
