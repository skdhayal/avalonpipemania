using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonPipeMania.Code
{
	static class MyExtensions
	{
	

		public static void DoIfAny<T>(this T e, Action<T> h)
			where T : class
		{
			if (e == null)
				return;

			h(e);
		}


	
	}

}
