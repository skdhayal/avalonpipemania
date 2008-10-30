using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonPipeMania.Code
{
	[Script]
	public class ExtendedField
	{
		public readonly Canvas Container;

		public readonly Field Field;

		public readonly Canvas Overlay;

		public ExtendedField(int SizeX, int SizeY, int Width, int Height)
		{
			this.Container = new Canvas
			{
				Width = Width,
				Height = Height
			};

			this.Container.ClipTo(0, 0, Width, Height);

			this.Field = new Field(SizeX, SizeY);

			this.Field.Container.AttachTo(this.Container);




			#region interactive layers
			var CurrentTileCanvas = new Canvas
			{
				Width = this.Field.Tiles.Width,
				Height = this.Field.Tiles.Height
			}.AttachTo(this.Container);


			var ExplosionCanvas = new Canvas
			{
				Width = Width,
				Height = Height
			}.AttachTo(this.Container);

			var InfoOverlay = new Canvas
			{
				Width = Width,
				Height = Height
			}.AttachTo(this.Container);

			this.Field.InfoOverlay.AttachTo(InfoOverlay);

			double CurrentTileX = 0;
			double CurrentTileY = 0;

			var CurrentTile = default(SimplePipe);

			// SimplePipe.BuildablePipes.Random()();

			this.GetPipeToBeBuilt = () => CurrentTile;

			this.SetPipeToBeBuilt =
				value =>
				{
					if (CurrentTile != null)
					{
						CurrentTile.OverlayBlackAnimationStop();
						CurrentTile.Container.Orphanize();
						CurrentTile.Container.Opacity = 1;
					}

					CurrentTile = value;

					if (CurrentTile != null)
					{
						CurrentTile.Container.Opacity = 0.7;
						CurrentTile.Container.MoveTo(CurrentTileX, CurrentTileY);
						CurrentTile.Container.AttachTo(CurrentTileCanvas);
						CurrentTile.OverlayBlackAnimationStart();
					}
				};

			#endregion


			#region overlay
			this.Overlay = new Canvas
			{
				Width = Width,
				Height = Height
			};

			var OverlayRectangle = new Rectangle
			{
				Fill = Brushes.Black,
				Width = Width,
				Height = Height,
				Opacity = 0
			}.AttachTo(Overlay);



			this.Field.Tiles.Overlay.AttachTo(Overlay);
			#endregion

			#region move the map with the mouse yet not too often anf smooth enough

			#region MoveTo
			Action<int, int> MoveTo = NumericEmitter.Of(
				(x, y) =>
				{
					this.Field.Tiles.Overlay.MoveTo(x, y);
					this.Field.Container.MoveTo(x, y);


					CurrentTileCanvas.MoveTo(x, y);
					ExplosionCanvas.MoveTo(x, y);
					InfoOverlay.MoveTo(x, y);
				}
			);
			#endregion

			#region CalculateMoveTo
			Action<int, int> CalculateMoveTo =
				(int_x, int_y) =>
				{
					double x = int_x;
					double y = int_y;

					//Console.WriteLine(new { x, y }.ToString());

					const int PaddingX = Tile.Size / 2;
					const int MarginLeft = Tile.ShadowBorder + Tile.Size + PaddingX;
					const int MarginWidth = MarginLeft + Tile.ShadowBorder + Tile.Size + PaddingX;
					var _x = PaddingX + (Width - (this.Field.Tiles.Width + PaddingX * 2)) * ((x - MarginLeft).Max(0) / (Width - MarginWidth)).Min(1);


					const int PaddingY = Tile.SurfaceHeight;
					const int MarginTop = Tile.ShadowBorder + Tile.SurfaceHeight + PaddingY;
					const int MarginHeight = MarginTop + Tile.ShadowBorder + Tile.Size + PaddingY;
					var _y = PaddingY + (Height - (this.Field.Tiles.Height + PaddingY * 2)) * ((y - MarginTop).Max(0) / (Height - MarginHeight)).Min(1);

					if (this.Field.Tiles.Width < Width)
						_x = (Width - this.Field.Tiles.Width) / 2;

					if (this.Field.Tiles.Height < Height)
						_y = (Height - this.Field.Tiles.Height) / 2;


					MoveTo(Convert.ToInt32(_x), Convert.ToInt32(_y));
				}
			;
			#endregion


			CalculateMoveTo(0, 0);


			Overlay.MouseMove +=
				(Sender, Arguments) =>
				{
					var p = Arguments.GetPosition(Overlay);

					CalculateMoveTo(Convert.ToInt32(p.X), Convert.ToInt32(p.Y));
				};
			#endregion




			#region IsBlockingPipe
			Func<bool> IsBlockingPipe =
				delegate
				{
					var u = this.Field.Tiles.FocusTile;

					if (u != null)
					{
						var q = this.Field[u];
						if (q != null)
						{
							var qt = q.GetType();
							if (!SimplePipe.BuildablePipeTypes.Any(t => t.Equals(qt)))
							{
								// we got a pipe on which we should not build upon
								return true;
							}


						}


						if (this.Field.ByIndex(u.IndexX, u.IndexY).Any(k => k.Value.HasWater))
							return true;
					}

					return false;
				};
			#endregion


			#region MouseMove
			Overlay.MouseMove +=
				(Sender, Arguments) =>
				{
					if (CurrentTile == null)
						return;

					var p = Arguments.GetPosition(this.Field.Tiles.Overlay);

					var u = this.Field.Tiles.FocusTile;

					if (IsBlockingPipe())
					{
						// we got a pipe on which we should not build upon
						u = null;
					}


					if (u != null)
					{
						p.X = 0 + u.IndexX * Tile.Size + Tile.ShadowBorder;
						p.Y = 0 + (u.IndexY + 1) * Tile.SurfaceHeight + Tile.ShadowBorder - Tile.Size;

						CurrentTile.Container.Opacity = 1;
					}
					else
					{
						p.X -= Tile.Size / 2;
						p.Y -= Tile.SurfaceHeight / 2;

						CurrentTile.Container.Opacity = 0.7;
					}

					CurrentTileX = p.X;
					CurrentTileY = p.Y;
					CurrentTile.Container.MoveTo(p.X, p.Y);
				};
			#endregion

			this.Field.Tiles.Click +=
				Target =>
				{
					if (CurrentTile == null)
						return;

					var u = this.Field.Tiles.FocusTile;

					if (u != null)
					{
						var q = this.Field[u];

						if (q != null)
							if (q.Input.Pump != null)
							{
								// we are clicking on a pump
								if (q.AddTimerAbort != null)
									q.AddTimerAbort();


								q.Input.Pump();
								return;

							}
					}

					if (IsBlockingPipe())
					{
						// we got a pipe on which we should not build upon
						u = null;
					}

					if (u != null)
					{
						var PipeToBeBuilt = this.PipeToBeBuilt;
						this.PipeToBeBuilt = null;


						if (this.Field.ByIndex(Target.IndexX, Target.IndexY).Any())
						{

							var px = u.IndexX * Tile.Size + Tile.ShadowBorder;
							var py = (u.IndexY + 1) * Tile.SurfaceHeight + Tile.ShadowBorder - Tile.Size;



							new Explosion().PlayAndOrphanize().Container.MoveTo(px, py).AttachTo(ExplosionCanvas);
						}


						this.Field[u] = PipeToBeBuilt;
						this.Field.RefreshPipes();

						if (PipeToBeBuiltUsed != null)
							PipeToBeBuiltUsed();
					}

				};
		}


		#region PipeToBeBuilt

		public event Action PipeToBeBuiltUsed;

		Action<SimplePipe> SetPipeToBeBuilt;
		Func<SimplePipe> GetPipeToBeBuilt;

		public SimplePipe PipeToBeBuilt
		{
			get
			{
				return GetPipeToBeBuilt();
			}
			set
			{
				SetPipeToBeBuilt(value);
			}
		}
		#endregion
	}
}
