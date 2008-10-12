using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Media;
using System.Windows.Shapes;
using AvalonPipeMania.Code.Assets;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class AnimationsTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;


		public AnimationsTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			#region background
			new[]
			{
				//Colors.White,
				//Colors.Blue,
				Colors.Red,
				Colors.Yellow
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



			KnownAnimations.Emoticons.ForEach(
				(value, index) =>
				{
					Console.WriteLine("ani: " + value.Prefix);

					value.ToAnimation().AttachContainerTo(this).MoveContainerTo(300 - value.Width / 2, 64 * (index + 1) - value.Height / 2);
				}
			);
		}
	}
}
