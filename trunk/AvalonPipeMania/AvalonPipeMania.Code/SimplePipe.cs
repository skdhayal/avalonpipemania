using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Threading;

namespace AvalonPipeMania.Code
{
	[Script]
	public partial class SimplePipe
	{
		[Script]
		public class Group
		{
			public Action Left;
			public Action Top;
			public Action Right;
			public Action Bottom;

			public Action Pump;
			public Action Drain;
		}

		public readonly Group Input = new Group();
		public readonly Group Output = new Group();

		public readonly Canvas Container = new Canvas();

		public bool HasWater
		{
			get
			{
				return this.PipeParts.Any(k => k.Water.Any(w => w.Visibility == System.Windows.Visibility.Visible));
			}
		}

		public Pipe[] PipeParts;

		public void OverlayBlackAnimationStart()
		{
			this.PipeParts.ForEach(k => k.OverlayBlackAnimationStart());
		}

		public void OverlayBlackAnimationStop()
		{
			this.PipeParts.ForEach(k => k.OverlayBlackAnimationStop());
		}

		public void Animate(IEnumerable<Image> Water, Action Done)
		{
			const int FrameRate = 1000 / 15;

			Water.ForEach(
				(Current, Next) =>
				{
					Current.Show();

					FrameRate.AtDelay(Next);
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
					typeof(SimplePipe.Vertical),
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

				//return new Func<SimplePipe>[]
				//{
				//    () => new SimplePipe.Horizontal(),
				//    () => new SimplePipe.Vertical(),
				//    () => new SimplePipe.Cross(),
				//    () => new SimplePipe.LeftToBottom(),
				//    () => new SimplePipe.RightToBottom(),
				//    () => new SimplePipe.TopToLeft(),
				//    () => new SimplePipe.TopToRight(),
				//};
			}
		}
	}
}
