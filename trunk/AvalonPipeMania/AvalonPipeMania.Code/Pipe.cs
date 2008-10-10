using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using AvalonPipeMania.Assets.Shared;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Windows.Media;

namespace AvalonPipeMania.Code
{
	[Script]
	public abstract partial class Pipe
	{
		// 1. fill color
		// 2. water
		// 3. glow
		public const double DefaultWaterOpacity = 0.8;



		public readonly Canvas Container;

		public const int Size = 64;

		public Pipe()
		{
			Container = new Canvas
			{
				Width = Size,
				Height = Size
			};
		}


		public Image Glow;
		
		public Image[] Water;



		// yellow:
		// hue +32
		// sat +154

		// green:
		// hue +92

		public Image Brown;
		public Image Green;
		public Image Yellow;

		public Image Outline;

		public Image OverlayBlack;

		DispatcherTimer OverlayBlackTimer;

		public Color Color
		{
			set
			{
				if (value == Colors.Green)
				{
					this.Green.Show();
					this.Yellow.Hide();
					this.Brown.Hide();
					return;
				}

				if (value == Colors.Yellow)
				{
					this.Green.Hide();
					this.Yellow.Show();
					this.Brown.Hide();
					return;
				}

				this.Green.Hide();
				this.Yellow.Hide();
				this.Brown.Show();
			}
		}

		public void OverlayBlackAnimationStart()
		{
			if (OverlayBlack == null)
				return;

			OverlayBlackAnimationStop();


			OverlayBlack.Opacity = 0.15;
			OverlayBlack.Show();

			OverlayBlackTimer = (1000 / 24).AtIntervalWithCounter(
				Counter =>
				{
					OverlayBlack.Opacity = (Math.Cos(Counter * 0.5) + 1) * 0.20 + 0.15; 
				}
			);
		}

		public void OverlayBlackAnimationStop()
		{
			if (OverlayBlack == null)
				return;

			if (OverlayBlackTimer != null)
			{
				OverlayBlackTimer.Stop();
				OverlayBlackTimer = null;
			}

			OverlayBlack.Hide();
		}

		[Script]
		public class Factory
		{
			public readonly Func<string, Image> ToImage;
			public readonly ParamsFunc<string, Image[]> ToWaterImages;
			public readonly ParamsFunc<string, Image[]> ToHiddenImages;

			public Factory(string Path, IAddChild Container)
			{
				this.ToImage =
					k => new Image
					{
						Source = (Path + "/" + k + ".png").ToSource(),
					}.AttachTo(Container);


				this.ToWaterImages =
					a =>
						a.ToArray(
							k => new Image
							{
								Source = (Path + "/" + k + ".png").ToSource(),
								Opacity = DefaultWaterOpacity,
								Visibility = Visibility.Hidden
							}.AttachTo(Container)
						);

				this.ToHiddenImages =
					a =>
						a.ToArray(
							k => new Image
							{
								Source = (Path + "/" + k + ".png").ToSource(),
								Visibility = Visibility.Hidden
							}.AttachTo(Container)
						);


			}
		}


		public static Pipe[] KnownPipes
		{
			get
			{
				return new Pipe[]
				{
					new LeftToBottom(),
					new LeftToDrain(),
					new LeftToRight(),
					new LeftToRightBent(),
					new PumpToLeft(),
					new PumpToRight(),
					new RightToBottom(),
					new RightToDrain(),
					new TopToBottom(),
					new TopToLeft(),
					new TopToRight()
				};
			}
		}
	}


}
