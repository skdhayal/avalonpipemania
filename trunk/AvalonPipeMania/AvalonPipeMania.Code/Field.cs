using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;


namespace AvalonPipeMania.Code
{
	[Script]
	public class Field
	{
		public readonly Canvas Container;

		public readonly TileField Tiles;

		public Field(int SizeX, int SizeY)
		{
			this.Tiles = new TileField(SizeX, SizeY);

			this.Container = new Canvas
			{
				Width = Tiles.Width,
				Height = Tiles.Height
			};

			this.Tiles.Container.AttachTo(this.Container);
		}
	}
}
