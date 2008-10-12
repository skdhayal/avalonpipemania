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
using AvalonPipeMania.Code.Fonts;

namespace AvalonPipeMania.Code
{
	partial class SimplePipe
	{



		public int WaterAnimationSpeed = 1000 / 50; // 64

		#region Animate

		public void AnimateLeftToRight(Image water, Action done)
		{
			Action FilteredDone = AnimationComplete(done);
			HasWater = true;

			water.ClipTo(0, 0, 0, 0);
			water.Show();

			Enumerable.Range(1, Pipe.Size / 2).ForEach(
				(Current_, Next) =>
				{
					var Current = Current_ * 2;
					water.ClipTo(
						0,
						0,
						Current,
						Pipe.Size
					);

					this.WaterAnimationSpeed.AtDelay(Next);
				}
			)(FilteredDone);
		}

		public void AnimateTopToBottom(Image water, Action done)
		{
			Action FilteredDone = AnimationComplete(done);
			HasWater = true;

			water.ClipTo(0, 0, 0, 0);
			water.Show();

			Enumerable.Range(1, Pipe.Size / 2).ForEach(
				(Current_, Next) =>
				{
					var Current = Current_ * 2;
					water.ClipTo(
						0,
						0,
						Pipe.Size,
						Current
					);

					this.WaterAnimationSpeed.AtDelay(Next);
				}
			)(FilteredDone);
		}

		public void AnimateBottomToTop(Image water, Action done)
		{
			Action FilteredDone = AnimationComplete(done);
			HasWater = true;

			water.ClipTo(0, 0, 0, 0);
			water.Show();

			Enumerable.Range(1, Pipe.Size / 2).ForEach(
				(Current_, Next) =>
				{
					var Current = Current_ * 2;
					water.ClipTo(
						0,
						Pipe.Size - Current,
						Pipe.Size,
						Current
					);

					this.WaterAnimationSpeed.AtDelay(Next);
				}
			)(FilteredDone);
		}



		public void AnimateRightToLeft(Image water, Action done)
		{
			Action FilteredDone = AnimationComplete(done);

			HasWater = true;

			water.ClipTo(0, 0, 0, 0);
			water.Show();

			Enumerable.Range(1, Pipe.Size / 2).ForEach(
				(Current_, Next) =>
				{
					var Current = Current_ * 2;

					water.ClipTo(
						Pipe.Size - Current,
						0,


						Current,
						Pipe.Size
					);

					this.WaterAnimationSpeed.AtDelay(Next);
				}
			)(FilteredDone);
		}

		public void Animate(IEnumerable<Image> water, Action done)
		{
			Action FilteredDone = AnimationComplete(done);
			HasWater = true;


			water.ForEach(
				(Current, Next) =>
				{
					// if there is water there already
					// we will stop here
					if (Current.Visibility == System.Windows.Visibility.Visible)
						return;

					Current.Show();

					(WaterAnimationSpeed * 4).AtDelay(Next);
				}
			)(FilteredDone);
		}
		#endregion

		int AnimationCompleteCounter = 0;
		int AnimationCompleteMultiplier = 100;

		Action AnimationComplete(Action done)
		{
			if (HasWater)
				return null;

			return delegate
			{
				AnimationCompleteCounter++;

			
				ShowBonusPoints(AnimationCompleteCounter * AnimationCompleteMultiplier);

				if (done != null)
					done();
			};
		}

		public void ShowBonusPoints(int BonusPoints)
		{
			if (BonusPoints == 0)
				return;

			var Label = default(BitmapLabel);


			bool a = false;
			int Framerate = (1000 / 24);

			if (BonusPoints < 0)
			{
				Label = new Fonts.showcard.RedNumbers { Text = (-BonusPoints) + "" };
				800.AtDelay(() => a = true);
				Framerate *= 2;

			}
			else
			{
				Label = new Fonts.showcard.WhiteNumbers { Text = BonusPoints + "" };
				500.AtDelay(() => a = true);
			}

			var x = (Pipe.Size - Label.Width) / 2;
			var y = (Pipe.Size - Label.Height) / 2;


			// center it
			Label.AttachContainerTo(this.InfoOverlay).MoveContainerTo(x, y);



			Framerate.AtIntervalWithTimer(
				t =>
				{
					y--;
					Label.MoveContainerTo(x, y);

					if (a)
					{
						Label.Container.Opacity -= 0.09;

						if (Label.Container.Opacity <= 0.09)
						{
							Label.OrphanizeContainer();
							t.Stop();
							return;
						}
					}
				}
			);
		}

	}
}
