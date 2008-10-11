using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using AvalonPipeMania.Assets.Shared;

namespace AvalonPipeMania.Code.Fonts.showcard
{
	[Script]
	public class WhiteNumbers : Fonts.BitmapLabel
	{
		public const string Path = (KnownAssets.Path.Fonts.showcard + "/Numbers_white.png");

		public WhiteNumbers()
			: base(Path.ToSource(), GetCharMap(), "")
		{

		}
		static CharMap GetCharMap()
		{
			return new CharMap
			{
				{ "1", 5, 3, 11, 17 },
				{ "2", 15, 3, 11, 17 },
				{ "3", 26, 3, 11, 17 },
				{ "4", 38, 3, 11, 17 },
				{ "5", 50, 3, 11, 17 },
				{ "6", 61, 3, 11, 17 },
				{ "7", 73, 3, 11, 17 },
				{ "8", 85, 3, 11, 17 },
				{ "9", 97, 3, 11, 17 },
				{ "0", 109, 3, 11, 17 },

			};
		}
	}
}
