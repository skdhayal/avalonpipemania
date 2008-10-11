using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Threading;
using System.Windows.Media;

namespace AvalonPipeMania.Code
{
	[Script]
	public partial class SimplePipe
	{

		public readonly Group Input = new Group();
		public readonly Group Output = new Group();
		public readonly Group SupportedOutput = new Group();

		public readonly Canvas Container = new Canvas();

		static readonly Action SupportedOutputMarker = delegate { };


		public bool IsVirtualPipe = false;

		public bool HasWater;



		public Pipe[] PipeParts;

		public void OverlayBlackAnimationStart()
		{
			this.PipeParts.ForEach(k => k.OverlayBlackAnimationStart());
		}

		public void OverlayBlackAnimationStop()
		{
			this.PipeParts.ForEach(k => k.OverlayBlackAnimationStop());
		}


		public Color Color
		{
			set
			{
				this.PipeParts.ForEach(k => k.Color = value);
			}
		}

		public int WaterAnimationSpeed = 1000 / 15;

		public void Animate(IEnumerable<Image> Water, Action Done)
		{
			HasWater = true;


			Water.ForEach(
				(Current, Next) =>
				{
					// if there is water there already
					// we will stop here
					if (Current.Visibility == System.Windows.Visibility.Visible)
						return;

					Current.Show();

					WaterAnimationSpeed.AtDelay(Next);
				}
			)(Done);
		}

		public static Type[] BuildablePipeTypes
		{
			get
			{
				return new[]
				{
					typeof(SimplePipe.Horizontal),
					typeof(SimplePipe.HorizontalWide),
					typeof(SimplePipe.Vertical),
					typeof(SimplePipe.VerticalWide),
					typeof(SimplePipe.Cross),
					typeof(SimplePipe.LeftToBottom),
					typeof(SimplePipe.RightToBottom),
					typeof(SimplePipe.TopToLeft),
					typeof(SimplePipe.TopToRight)
				};
			}
		}

		public static Func<SimplePipe>[] BuildablePipes
		{
			get
			{
				return BuildablePipeTypes.ToArray<Type, Func<SimplePipe>>(
					k => () => (SimplePipe)Activator.CreateInstance(k)
				);
			}
		}


		public Action AddTimerAbort;

		public void AddTimer(int timeout, Action done)
		{
			if (AddTimerAbort != null)
				AddTimerAbort();

			var white = new Fonts.showcard.WhiteNumbers();
			var yellow = new Fonts.showcard.YellowNumbers();
			var red = new Fonts.showcard.RedNumbers();

			

			Action<int> Update =
				value =>
				{
					Action<Fonts.BitmapLabel> Show =
						(label) =>
						{
							label.Container.Show();
							label.Text = value + "";
						};

					if (value < 4)
					{
						white.Container.Hide();
						yellow.Container.Hide();
						Show(red);
					}
					else if ((value - 4) < (timeout - 4) / 4)
					{
						white.Container.Hide();
						Show(yellow);
						red.Container.Hide();
					}
					else
					{
						Show(white);
						yellow.Container.Hide();
						red.Container.Hide();
					}
				};


			var counter = timeout;

			Update(counter);

			var t = 1000.AtIntervalWithTimer(
				k =>
				{
					if (counter == 0)
					{
						AddTimerAbort();
						return;
					}

					counter--;
					Update(counter);

					if (counter == 0)
					{
						done();
					}
				}
			);


			AddTimerAbort =
				delegate
				{
					AddTimerAbort = null;

					white.Container.Orphanize();
					yellow.Container.Orphanize();
					red.Container.Orphanize();
					t.Stop();
				};


			white.Container.AttachTo(this.Container);
			yellow.Container.AttachTo(this.Container);
			red.Container.AttachTo(this.Container);

		}
	}
}
