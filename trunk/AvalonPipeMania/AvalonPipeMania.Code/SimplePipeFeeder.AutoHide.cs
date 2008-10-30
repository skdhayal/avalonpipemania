using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonPipeMania.Code
{
	partial class SimplePipeFeeder 
	{
		[Script]
		public class AutoHide
		{
			public AutoHide(SimplePipeFeeder Feeder, UIElement Overlay, int DefaultWidth, int DefaultHeight)
			{
				var state = FeederState.LeftAndIdle;


				var feeder_x = Tile.ShadowBorder;
				var feeder_y = Tile.ShadowBorder + Tile.Size;



				Action<int, int, Action> feeder_MoveTo = NumericEmitter.Of(
					(x, y) =>
					{
						Feeder.Container.MoveTo(x, y);
					}
				);

				feeder_MoveTo(feeder_x, feeder_y, null);

				Overlay.MouseMove +=
					(Sender, Arguments) =>
					{
						var p = Arguments.GetPosition(Overlay);

						if (p.X < feeder_x + Pipe.Size)
						{
							#region LeftAndIdle
							if (state == FeederState.LeftAndIdle)
							{
								state = FeederState.LeftAndMoving;

								feeder_MoveTo(-Pipe.Size * 2, feeder_y,
									delegate
									{

										feeder_MoveTo = NumericEmitter.Of(
											(x, y) =>
											{
												Feeder.Container.MoveTo(x, y);
											}
										);

										Console.WriteLine("to right");
										feeder_MoveTo(DefaultWidth + Pipe.Size, feeder_y,
											delegate
											{

												feeder_MoveTo(DefaultWidth - Pipe.Size - feeder_x, feeder_y,
													delegate
													{
														Console.WriteLine("to right done");

														state = FeederState.RightAndIdle;
													}
												);
											}
										);
									}
								);

							}
							#endregion

							return;
						}

						if (p.X > DefaultWidth - feeder_x - Pipe.Size)
						{
							if (state == FeederState.RightAndIdle)
							{
								state = FeederState.RightAndMoving;

								feeder_MoveTo(DefaultWidth + Pipe.Size, feeder_y,
									delegate
									{

										feeder_MoveTo = NumericEmitter.Of(
											(x, y) =>
											{
												Feeder.Container.MoveTo(x, y);
											}
										);

										Console.WriteLine("to left");
										feeder_MoveTo(-Pipe.Size * 2, feeder_y,
											delegate
											{

												feeder_MoveTo(feeder_x, feeder_y,
													delegate
													{
														Console.WriteLine("to left done");
														state = FeederState.LeftAndIdle;
													}
												);
											}
										);

									}
								);
							}

							return;
						}
					};
			}


			public enum FeederState
			{
				LeftAndIdle,
				LeftAndMoving,
				RightAndIdle,
				RightAndMoving
			}
		}
	}
}
