﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using AvalonPipeMania.Assets.Shared;
using System.Windows;

namespace AvalonPipeMania.Code
{
	partial class Pipe
	{
	

		[Script]
		public class LeftToBottom : Pipe
		{
			public LeftToBottom()
			{
				var f = new Factory(KnownAssets.Path.Pipe.LeftToBottom, this.Container);

				this.Outline = f.ToImage("outline");

				this.Brown = f.ToImage("brown");
				this.Brown.Visibility = Visibility.Hidden;

				this.Green = f.ToImage("green");
				this.Green.Visibility = Visibility.Hidden;

				this.Yellow = f.ToImage("yellow");

				this.Water = f.ToWaterImages(
					"0_8",
					"8_16",
					"16_24",
					"24_32",
					"32_40",
					"40_48",
					"48_56",
					"56_64"
				);

				this.OverlayBlack = f.ToImage("black");
				this.OverlayBlack.Visibility = Visibility.Hidden;

				this.Glow = f.ToImage("glow");
			}
		}

		
	}


}
