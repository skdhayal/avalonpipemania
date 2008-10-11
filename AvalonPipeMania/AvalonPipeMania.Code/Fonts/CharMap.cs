using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ScriptCoreLib;

namespace AvalonPipeMania.Code.Fonts
{
	[Script]
	public class CharMap : IEnumerable<KeyValuePair<string, Rect>>
	{
		public CharMap()
		{

		}

		readonly Dictionary<string, Rect> Map = new Dictionary<string, Rect>();

		public void Add(string c, int x, int y, int w, int h)
		{
			Map[c] = new Rect { X = x, Y = y, Width = w, Height = h };
		}

		#region IEnumerable<KeyValuePair<string,Rect>> Members

		public IEnumerator<KeyValuePair<string, Rect>> GetEnumerator()
		{
			return this.Map.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.Map.GetEnumerator();
		}

		#endregion

		public bool Contains(string c)
		{
			return this.Map.ContainsKey(c);
		}

		public Rect this[string c]
		{
			get
			{
				return this.Map[c];

			}
		}
	}

}
