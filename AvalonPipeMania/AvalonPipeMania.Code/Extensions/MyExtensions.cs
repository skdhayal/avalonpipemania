using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;

namespace AvalonPipeMania.Code.Extensions
{
	[Script]
	public static class MyExtensions
	{
		public static Action ShowAndHideLater(this UIElement e)
		{
			e.Show();

			return e.Hide;
		}
	}
}
