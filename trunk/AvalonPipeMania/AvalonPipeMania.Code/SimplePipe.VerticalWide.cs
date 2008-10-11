using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonPipeMania.Code
{
	partial class SimplePipe
	{

		[Script]
		public class VerticalWide : SimplePipe
		{
			readonly Pipe.TopToBottomWide PipeTopToBottom;

			public VerticalWide()
			{
				this.PipeTopToBottom = new Pipe.TopToBottomWide();

				this.PipeTopToBottom.Container.AttachTo(this.Container);



				// if the animation has already been started or even if its already
				// complete this action should not be called again.

				this.SupportedOutput.Bottom = SupportedOutputMarker;
				this.Input.Top =
					delegate
					{
						var water = this.PipeTopToBottom.Water.First();

						water.ClipTo(0, 0, 0, 0);
						water.Show();

						Enumerable.Range(0, Pipe.Size).ForEach(
							(Current, Next) =>
							{
								water.ClipTo(0, 0, Pipe.Size, Current);

								this.WaterAnimationSpeed.AtDelay(Next);
							}
						)(this.Output.Bottom);
					};

				this.SupportedOutput.Top = SupportedOutputMarker;
				this.Input.Bottom =
					delegate
					{
						var water = this.PipeTopToBottom.Water.Last();

						water.ClipTo(0, 0, 0, 0);
						water.Show();

						Enumerable.Range(0, Pipe.Size).ForEach(
							(Current, Next) =>
							{
								water.ClipTo(0, Pipe.Size - Current, Pipe.Size, Current);

								this.WaterAnimationSpeed.AtDelay(Next);
							}
						)(this.Output.Top);
					};


				this.PipeParts = new Pipe[]
				{
					this.PipeTopToBottom
				};
			}
		}
	}
}
