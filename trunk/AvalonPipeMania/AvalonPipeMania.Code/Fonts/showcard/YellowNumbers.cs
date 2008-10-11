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
	public class YellowNumbers : NumbersBase
	{
		public YellowNumbers()
			: base((KnownAssets.Path.Fonts.showcard + "/Numbers_yellow.png").ToSource())
		{

		}
		
	}
}
