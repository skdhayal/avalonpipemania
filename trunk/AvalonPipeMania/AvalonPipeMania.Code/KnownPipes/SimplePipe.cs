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
	public partial class SimplePipe : ISupportsContainer, IDisposable
	{

		public int DemolitionCost = -50;

		public readonly Group Input = new Group();
		public readonly Group Output = new Group();
		public readonly Group SupportedOutput = new Group();

		public Canvas Container { get; set; }

		public Canvas InfoOverlay;

		static readonly Action SupportedOutputMarker = delegate { };

		public bool IsVirtualPipe = false;

		public bool HasWater;



		public Pipe[] PipeParts;


		public SimplePipe()
		{
			this.Container = new Canvas
			{
				Width = Pipe.Size,
				Height = Pipe.Size
			};

			this.InfoOverlay = new Canvas
			{
				Width = Pipe.Size,
				Height = Pipe.Size
			};
		}

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

		#region IDisposable Members

		protected bool IsDisposed;

		public event Action Disposing;

		public void Dispose()
		{
			IsDisposed = true;
			this.Container.Orphanize();
			this.InfoOverlay.Orphanize();

			if (Disposing != null)
				Disposing();
		}

		#endregion
	}
}
