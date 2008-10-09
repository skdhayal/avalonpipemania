using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class InteractiveFieldTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public InteractiveFieldTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;

			var f = new Field(6, 4);

			#region round the corners
			f.Tiles[0, 0].Hide();
			f.Tiles[f.Tiles.SizeX - 1, 0].Hide();
			f.Tiles[f.Tiles.SizeX - 1, f.Tiles.SizeY - 1].Hide();
			f.Tiles[0, f.Tiles.SizeY - 1].Hide();
			#endregion

			f[2, 0] = new SimplePipe.PumpToLeft();

			f[1, 0] = new SimplePipe.RightToBottom();
			f[1, 1] = new SimplePipe.Vertical();
			f[1, 2] = new SimplePipe.TopToRight();
			f[2, 2] = new SimplePipe.Cross();
			f[3, 2] = new SimplePipe.Horizontal();
			f[4, 2] = new SimplePipe.LeftToDrain();

			f[2, 1] = new SimplePipe.RightToBottom();
			f[2, 3] = new SimplePipe.TopToLeft();
			f[3, 1] = new SimplePipe.Horizontal();
			f[4, 1] = new SimplePipe.TopToLeft();
			f[4, 0] = new SimplePipe.LeftToBottom();
			f[3, 0] = new SimplePipe.RightToDrain();
			f[1, 3] = new SimplePipe.PumpToRight();

			f[4, 2].Output.Drain = f[1, 3].Input.Pump;

			// show a hole in the floor
			f.Tiles[3, 0].Drain.Visibility = System.Windows.Visibility.Visible;
			f.Tiles[4, 2].Drain.Visibility = System.Windows.Visibility.Visible;

			// user must click on the pump to activate it
			f.Tiles[2, 0].Overlay.MouseLeftButtonUp +=
				delegate
				{
					f[2, 0].Input.Pump();
				};



			// when a user adds a pipe on top of another
			// we need to call this to force a zorder sort
			f.RefreshPipes();

			var x = (DefaultWidth - f.Tiles.Width) / 2;
			var y = (DefaultHeight - f.Tiles.Height) / 2;

			f.Container.MoveTo(x, y).AttachTo(this);

			var BuildablePipes = SimplePipe.BuildablePipes.AsCyclicEnumerable();

			var CurrentTile = BuildablePipes.First()();

			CurrentTile.Container.Opacity = 0.7;
			CurrentTile.Container.AttachTo(this);
			CurrentTile.OverlayBlackAnimationStart();

			#region overlay
			var Overlay = new Canvas
			{
				Width = DefaultWidth,
				Height = DefaultHeight,
			}.AttachTo(this);

			new Rectangle
			{
				Fill = Brushes.White,
				Width = DefaultWidth,
				Height = DefaultHeight,
				Opacity = 0
			}.AttachTo(Overlay);

			f.Tiles.Overlay.MoveTo(x, y).AttachTo(Overlay);
			#endregion

			f.Tiles.Click +=
				Target =>
				{
					CurrentTile.OverlayBlackAnimationStop();
					CurrentTile.Container.FadeOut();
				};

			#region MouseMove
			this.MouseMove +=
				(Sender, Arguments) =>
				{
					var p = Arguments.GetPosition(this);

					if (f.Tiles.FocusTile != null)
					{
						p.X = x + f.Tiles.FocusTile.IndexX * Tile.Size + Tile.ShadowBorder;
						p.Y = y + (f.Tiles.FocusTile.IndexY + 1) * Tile.SurfaceHeight + Tile.ShadowBorder - Tile.Size;
						
						CurrentTile.Container.Opacity = 1;
					}
					else
					{
						p.X -= Tile.Size / 2;
						p.Y -= Tile.SurfaceHeight / 2;

						CurrentTile.Container.Opacity = 0.7;
					}

					CurrentTile.Container.MoveTo(p.X , p.Y );
				};
			#endregion

		}
	}
}
