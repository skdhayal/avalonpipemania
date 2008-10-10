using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;

namespace AvalonPipeMania.Code
{
	[Script]
	public class TileField
	{
		public readonly Canvas Shadow;
		public readonly Canvas Content;
		public readonly Canvas Overlay;
		public readonly Canvas Container;

		public readonly int Width;
		public readonly int Height;

		public readonly List<Tile> Tiles = new List<Tile>();

		public readonly int SizeX;
		public readonly int SizeY;

		public TileField(int SizeX, int SizeY)
		{
			this.SizeX = SizeX;
			this.SizeY = SizeY;

			this.Width = Tile.Size * SizeX + Tile.ShadowBorder * 2;
			this.Height = Tile.SurfaceHeight * SizeY + Tile.ShadowBorder * 2;

			this.Shadow = new Canvas
			{
				Width = this.Width,
				Height = this.Height,
			};

			this.Content = new Canvas
			{
				Width = this.Width,
				Height = this.Height,
			};

			this.Overlay = new Canvas
			{
				Width = this.Width,
				Height = this.Height,
			};

			this.Container = new Canvas
			{
				Width = this.Width,
				Height = this.Height,
			};

			this.Shadow.AttachTo(this.Container);
			this.Content.AttachTo(this.Container);

			for (int ix = 0; ix < SizeX; ix++)
				for (int iy = 0; iy < SizeY; iy++)
				{
					var tile = new Tile
					{
						IndexX = ix,
						IndexY = iy
					};

					Tiles.Add(tile);

					tile.Shadow.MoveTo(64 * ix, 52 * iy).AttachTo(this.Shadow);
					tile.Container.MoveTo(64 * ix + Tile.ShadowBorder, 52 * iy + Tile.ShadowBorder).AttachTo(this.Content);
					tile.Overlay.MoveTo(64 * ix + Tile.ShadowBorder, 52 * iy + Tile.ShadowBorder).AttachTo(this.Overlay);

		

					tile.Overlay.MouseEnter +=
						delegate
						{
							FocusTile = tile;

							if (Focus != null)
								Focus(tile);

							//tile.YellowFilter.Visibility = System.Windows.Visibility.Visible;
							tile.Select.Show();
						};
				

					tile.Overlay.MouseLeave +=
						delegate
						{
							FocusTile = null;

							if (Unfocus != null)
								Unfocus(tile);


							tile.Select.Hide();
							//tile.YellowFilter.Visibility = System.Windows.Visibility.Hidden;
						};
				}



			this.Overlay.MouseLeftButtonUp +=
				delegate
				{
					if (Click != null)
						Click(FocusTile);
				};
		}

		public Tile FocusTile;

		public event Action<Tile> Focus;
		public event Action<Tile> Unfocus;
		public event Action<Tile> Click;

		public Tile this[int x, int y]
		{
			get
			{
				return this.Tiles.Single(
					k =>
					{
						if (k.IndexX != x)
							return false;

						if (k.IndexY != y)
							return false;

						return true;
					}
				);
			}
		}
	}
}
