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


		public Action OverlayBlackAnimationStart = delegate { };
		public Action OverlayBlackAnimationStop = delegate { };

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

	}
}
