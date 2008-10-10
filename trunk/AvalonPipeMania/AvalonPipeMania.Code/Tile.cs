using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonPipeMania.Assets.Shared;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;

namespace AvalonPipeMania.Code
{
	[Script]
	public class Tile
	{
		private static Random rnd = new Random();

		public readonly Canvas Container;

		public const int ShadowBorder = 8;
		public const int Size = 64;
		public const int SurfaceHeight = 52;

		public readonly Rectangle Overlay;

		public readonly Image Select;

		public readonly Image YellowFilter;
		
		public readonly Image Shadow;

		//public double MaxBlackFilterOpacity = 0.7;
		public double MaxBlackFilterOpacity = 0.4;

		public readonly Image Drain;

		public int IndexX;
		public int IndexY;


		public readonly Image BackgroundPink;
		public readonly Image BackgroundBrown;
		public readonly Image BackgroundGray;
		public readonly Image BackgroundCyan;

		public Color Color
		{
			set
			{
				if (value == Colors.Cyan)
				{
					BackgroundCyan.Show();
					BackgroundGray.Hide();
					BackgroundPink.Hide();
					BackgroundBrown.Hide();

					return;
				}

				if (value == Colors.Gray)
				{
					BackgroundCyan.Hide();
					BackgroundGray.Show();
					BackgroundPink.Hide();
					BackgroundBrown.Hide();

					return;
				}

				if (value == Colors.Pink)
				{
					BackgroundCyan.Hide();
					BackgroundGray.Hide();
					BackgroundPink.Show();
					BackgroundBrown.Hide();

					return;
				}

				BackgroundCyan.Hide();
				BackgroundGray.Hide();
				BackgroundPink.Hide();
				BackgroundBrown.Show();
			}
		}

		public Tile()
		{
			this.Container = new Canvas
			{
				Width = Size,
				Height = Size
			};

			this.Shadow = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_black_unfocus8.png").ToSource(),
			};


			this.BackgroundBrown = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0.png").ToSource(),
			}.AttachTo(this.Container);

			this.BackgroundPink = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_pink.png").ToSource(),
				Visibility = System.Windows.Visibility.Hidden
			}.AttachTo(this.Container);

			this.BackgroundGray = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_gray.png").ToSource(),
				Visibility = System.Windows.Visibility.Hidden
			}.AttachTo(this.Container);

			this.BackgroundCyan = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_cyan.png").ToSource(),
				Visibility = System.Windows.Visibility.Hidden
			}.AttachTo(this.Container);


			var BlackFilter = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_black.png").ToSource(),
				Opacity = rnd.NextDouble() * MaxBlackFilterOpacity
			}.AttachTo(this.Container);

			this.Drain = new Image
			{
				Source = (KnownAssets.Path.Data + "/drain.png").ToSource(),
				Visibility = System.Windows.Visibility.Hidden
			}.MoveTo(0, -12).AttachTo(this.Container);


			this.Select = new Image
			{
				Source = (KnownAssets.Path.Data + "/select0.png").ToSource(),
				Visibility = System.Windows.Visibility.Hidden,
			}.AttachTo(this.Container);

			this.YellowFilter = new Image
			{
				Source = (KnownAssets.Path.Data + "/tile0_yellow.png").ToSource(),
				Opacity = 0.4,
				Visibility = System.Windows.Visibility.Hidden,
			}.AttachTo(this.Container);


			this.Overlay = new Rectangle
			{
				Cursor = Cursors.Hand,
				Fill = Brushes.Black,
				Width = Size,
				Height = 52,
				Opacity = 0
			}.MoveTo(ShadowBorder, ShadowBorder);
		}

		public void Show()
		{
			this.Shadow.Show();
			this.Container.Show();
			this.Overlay.Show();
		}


		public void Hide()
		{
			this.Shadow.Hide();
			this.Container.Hide();
			this.Overlay.Hide();
		}
	}
}
