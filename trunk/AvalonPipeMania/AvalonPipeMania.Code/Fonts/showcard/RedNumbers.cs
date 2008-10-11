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
	public class RedNumbers : NumbersBase
	{
		public RedNumbers()
			: base((KnownAssets.Path.Fonts.showcard + "/Numbers_red.png").ToSource())
		{

		}
		
	}
}
