using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;

namespace AvalonPipeMania.Code.Extensions
{
	[Script]
	public class ASCIIImage
	{
		public readonly string[] Lines;

		public readonly int Height;
		public readonly int Width;

		public ASCIIImage(string value)
			: this(value, 2, 1)
		{
		}

		public ASCIIImage(string value, int skipx, int skipy)
			: this(value, skipx, skipy, 1, 1)
		{
		}

		public ASCIIImage(string value, int skipx, int skipy, int MultiplyX, int MultiplyY)
		{
			var lines = value.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

			var a = new List<string>();
			for (int i = 0; i < lines.Length; i += 1 + skipy)
			{
				var v = lines[i];
				var n = "";

				for (int j = 0; j < v.Length; j += 1 + skipx)
				{
					for (int mj = 0; mj < MultiplyX; mj++)
						n += v.Substring(j, 1);
				}

				for (int mi = 0; mi < MultiplyY; mi++)
					a.Add(n);
			}

			this.Lines = a.ToArray();

			this.Height = this.Lines.Length;

			if (Height > 0)
				this.Width = this.Lines[0].Length;
		}

		public string this[int x, int y]
		{
			get
			{
				return this.Lines[y].Substring(x, 1);
			}
		}
	}

}
