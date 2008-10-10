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

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class ColorTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;


		public ColorTest()
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


			// what do we need to do:
			// show all possible pipe buttons
			// make them selectable and placeable

			const int PipeSizeWithMargin = Pipe.Size + 8;

			Action<int, Action<Pipe>> AddPipes =
				(offset, handler) =>
				{
					Pipe.KnownPipes.ForEach(
						(pipe, i) =>
						{
							pipe.Container.AttachTo(this).MoveTo(16 + (i % 2) * PipeSizeWithMargin + offset, 32 + PipeSizeWithMargin * i / 2);

							handler(pipe);
						}
					);
				};


			AddPipes(0,
				pipe =>
				{
					pipe.Color = Colors.Transparent;

				}
			);

			AddPipes(PipeSizeWithMargin * 2,
				pipe =>
				{
					pipe.Color = Colors.Yellow;
				}
			);

			AddPipes(PipeSizeWithMargin * 4,
				pipe =>
				{
					pipe.Color = Colors.Green;

				}
			);

			AddPipes(PipeSizeWithMargin * 6,
				pipe =>
				{
					pipe.Color = Colors.Brown;
				}
			);
		}
	}
}
