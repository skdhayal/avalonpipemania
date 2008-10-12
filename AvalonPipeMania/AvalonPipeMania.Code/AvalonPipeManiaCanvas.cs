using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonPipeMania.Assets.Shared;
using AvalonPipeMania.Code.Labs;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Cursors;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Avalon.TextButton;
using ScriptCoreLib.Shared.Avalon.TiledImageButton;
using ScriptCoreLib.Shared.Lambda;
using AvalonPipeMania.Code.Extensions;
using System.Windows.Threading;

namespace AvalonPipeMania.Code
{
	[Script]
	public partial class AvalonPipeManiaCanvas : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public Action<string> PlaySound = delegate { };

		// See more:
		// http://members.chello.at/theodor.lauppert/games/pipe.htm

		// tile themes to add:
		// moon lander/ space
		// grass/farmer/trees

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



			// we selecting an implementation here:
			// this should be dynamic in the UI for labs entrypoint
			var Options = new Dictionary<Type, Func<Canvas>>
			{
				{ typeof(TileFieldTest), 
					() => new TileFieldTest
					{
						PlaySound = e => PlaySound(e),
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(ColorTest), 
					() => new ColorTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(SimplePipeTest),
					() => new SimplePipeTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(FieldTest),
					() => new FieldTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(InteractiveFieldTest),
					() => new InteractiveFieldTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(ScrollTest),
					() => new ScrollTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(ExtendedFieldTest),
					() => new ExtendedFieldTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(ColoredFieldTest),
					() => new ColoredFieldTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(SpaceInvaderTest),
					() => new SpaceInvaderTest
					{
						Visibility = Visibility.Hidden
					}
				},
				{ typeof(AnimationsTest),
					() => new AnimationsTest
					{
						Visibility = Visibility.Hidden
					}
				},
			};

			var Content = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);



			var Buttons = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight
			}.AttachTo(this);

			var Navigationbar = new AeroNavigationBar();

			Navigationbar.Container.MoveTo(4, 4).AttachTo(this);

			#region generate the menu
			const int ButtonHeight = 30;

			Options.ForEach(
				(Option, Index) =>
				{
					var x = 72;
					var y = 16 + Index * ButtonHeight;

					var Button = new TextButtonControl
					{
						Text = (Index + 1) + ". Open " + Option.Key.Name,
						Width = 200,
						Height = ButtonHeight
					};

					Button.Background.Fill = Brushes.Black;
					Button.Background.Opacity = 0.5;
					Button.Foreground = Brushes.Blue;

					Button.MouseEnter +=
						delegate
						{


							Button.Background.Fill = Brushes.Blue;
							Button.Background.Opacity = 0.5;
							Button.Foreground = Brushes.White;
						};

					Button.MouseLeave +=
						delegate
						{
							Button.Background.Fill = Brushes.Black;
							Button.Background.Opacity = 0.5;
							Button.Foreground = Brushes.Blue;
						};

					var OptionCanvas = default(Canvas);

					Button.Click +=
						delegate
						{
							if (OptionCanvas == null)
								OptionCanvas = Option.Value();

							Navigationbar.History.Add(
								delegate
								{
									OptionCanvas.Orphanize().Hide();
									Buttons.Show();
								},
								delegate
								{
									OptionCanvas.AttachTo(Content).Show();
									Buttons.Hide();
								}
							);
						};

					Button.Container.MoveTo(x, y).AttachTo(Buttons);
				}
			);
			#endregion


		


		}

	}
}
