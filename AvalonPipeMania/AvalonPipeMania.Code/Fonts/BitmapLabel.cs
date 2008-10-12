using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ScriptCoreLib;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonPipeMania.Code.Fonts
{
	[Script]
	public class BitmapLabel : ISupportsContainer
	{
		public Canvas Container { get; set; }

		public readonly ImageSource Source;
		public readonly CharMap CharMap;

		public int Width;
		public int Height;

		public BitmapLabel(ImageSource source, CharMap c, string value)
		{
			this.Container = new Canvas();

			this.Source = source;
			this.CharMap = c;
			this.Text = value;
		}

		Image[] _Images;
		string _Text;

		public string Text
		{
			get { return _Text; }
			set
			{


				_Text = value;

				var a = new Image[value.Length];

				if (_Images != null)
				{
					for (int i = 0; i < _Images.Length; i++)
					{
						if (i < a.Length)
						{
							a[i] = _Images[i];
						}
						else
						{
							_Images[i].Orphanize();
						}
					}
				}

				var x = 0;
				var h = 0;

				for (int i = 0; i < value.Length; i++)
				{
					var s = value.Substring(i, 1);

					if (this.CharMap.Contains(s))
					{
						var m = this.CharMap[s];

						var img = a[i];

						if (img == null)
						{
							img = new Image
							{
								Source = this.Source
							}.AttachTo(Container);

							a[i] = img;
						}

						img.ClipTo(m);
						img.MoveTo(-m.X + x, -m.Y);

						x += Convert.ToInt32(m.Width);

						h = h.Max(Convert.ToInt32(m.Height));
					}

				}

				Width = x;
				this.Container.Width = x;
				Height = h;
				this.Container.Height = h;


				_Images = a;
			}
		}
	}

}
