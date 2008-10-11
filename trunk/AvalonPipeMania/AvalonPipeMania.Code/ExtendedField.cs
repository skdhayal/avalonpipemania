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
			Action<int, int> MoveTo = Tween.NumericEmitter.Of(
				(x, y) =>
				{
					this.Field.Tiles.Overlay.MoveTo(x, y);
					this.Field.Container.MoveTo(x, y);
				}
			);

			Action<int, int> CalculateMoveTo =
				//Tween.NumericOmitter.Of(
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
				//)
			;
			#endregion


			Overlay.MouseMove +=
				(Sender, Arguments) =>
				{
					var p = Arguments.GetPosition(Overlay);

					CalculateMoveTo(Convert.ToInt32(p.X), Convert.ToInt32(p.Y));
				};
		}
	}
}
