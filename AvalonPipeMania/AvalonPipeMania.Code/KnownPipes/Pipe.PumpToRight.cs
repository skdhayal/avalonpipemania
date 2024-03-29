﻿using System;
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
		public class PumpToRight : Pipe
		{
			public readonly DispatcherTimer PumpHandleAnimation;

			public PumpToRight()
			{
				{
					var f = new Factory(KnownAssets.Path.Pipe.PumpToRight, this.Container);

					this.Outline = f.ToImage("outline");

					this.Brown = f.ToImage("brown");
					this.Brown.Visibility = Visibility.Hidden;

					this.Green = f.ToImage("green");
					this.Green.Visibility = Visibility.Hidden;

					this.Yellow = f.ToImage("yellow");


					this.Water = f.ToWaterImages(
						"0_8",
						"8_16"
					);

					this.Glow = f.ToImage("glow");
				}

				{
					var f = new Factory(KnownAssets.Path.Pipe.PumpHandle, this.Container);

					var Handles = f.ToHiddenImages(
						"1", "2", "3", "4", "5", "6"
					);

					Handles.AtModulus(0).Visibility = Visibility.Visible;
					Action Hide = () => Handles.AtModulus(0).Visibility = Visibility.Hidden;

					this.PumpHandleAnimation = (1000 / 23).AtIntervalWithCounter(
						Counter =>
						{
							Hide();

							Handles.AtModulus(Counter).Visibility = Visibility.Visible;
							Hide = () => Handles.AtModulus(Counter).Visibility = Visibility.Hidden;
						}
					);

				}
			}
		}


	}


}
