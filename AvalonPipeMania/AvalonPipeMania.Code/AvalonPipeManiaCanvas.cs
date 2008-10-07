using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using AvalonPipeMania.Assets.Shared;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Cursors;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.TiledImageButton;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using AvalonPipeMania.Code.Labs;
using ScriptCoreLib.Shared.Avalon.TextButton;

namespace AvalonPipeMania.Code
{
	[Script]
	public partial class AvalonPipeManiaCanvas : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public Action<string> PlaySound = delegate { };

		public AvalonPipeManiaCanvas()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			#region background
			new[]
			{
				//Colors.White,
				//Colors.Blue,
				Colors.Black,
				Colors.White,
				Colors.Black
			}.ToGradient(DefaultHeight / 4).ForEach(
				(c, i) =>
				new Rectangle
				{
					Fill = new SolidColorBrush(c),
					Width = DefaultWidth,
					Height = 5,
				}.MoveTo(0, i * 4).AttachTo(this)
			);
			#endregion

			var Buttons = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);

			// we selecting an implementation here:
			// this should be dynamic in the UI for labs entrypoint
			var Options = new Canvas[]
			{
				new FieldTest
				{
					PlaySound = e => PlaySound(e),
					Visibility = Visibility.Hidden
				}.AttachTo(this),

				new ColorTest
				{
					Visibility = Visibility.Hidden
				}.AttachTo(this)
			};

			var Navigationbar = new AeroNavigationBar();

			Navigationbar.Container.MoveTo(4, 4).AttachTo(this);

			#region generate the menu
			const int ButtonHeight = 30;

			Options.ForEach(
				(Option, Index) =>
				{
					var Button = new TextButtonControl
					{
						Text = "Open " + Option.GetType().Name,
						Width = 200,
						Height = ButtonHeight
					};

					Button.Background.Fill = Brushes.Blue;
					Button.Background.Opacity = 0;
					Button.Foreground = Brushes.Blue;

					Button.MouseEnter +=
						delegate
						{
							Button.Background.Opacity = 0.5;
							Button.Foreground = Brushes.White;
						};

					Button.MouseLeave +=
						delegate
						{
							Button.Background.Opacity = 0;
							Button.Foreground = Brushes.Blue;
						};


					Button.Click +=
						delegate
						{
							Navigationbar.History.Add(
								delegate
								{
									Option.Visibility = Visibility.Hidden;
									Buttons.Visibility = Visibility.Visible;
								},
								delegate
								{
									Option.Visibility = Visibility.Visible;
									Buttons.Visibility = Visibility.Hidden;
								}
							);
						};

					Button.Container.MoveTo(72, 16 + Index * ButtonHeight).AttachTo(Buttons);
				}
			);
			#endregion


		}
	}
}
