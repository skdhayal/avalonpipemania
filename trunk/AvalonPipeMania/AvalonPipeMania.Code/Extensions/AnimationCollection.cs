using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using System.Windows;

namespace AvalonPipeMania.Code.Extensions
{
	[Script]
	public class AnimationCollection : IEnumerable<AnimationCollection.AnimationInfo>
	{
		readonly Func<string, string[]> PrefixToFrames;

		public AnimationCollection(Func<string, string[]> PrefixToFrames)
		{
			this.PrefixToFrames = PrefixToFrames;
		}

		[Script]
		public class AnimationInfo
		{
			public int Framerate;
			public int Width;
			public int Height;
			public string Prefix;

			public string[] Frames;

			public Animation ToAnimation()
			{
				return new Animation(this);
			}

			public static implicit operator Animation(AnimationInfo e)
			{
				return e.ToAnimation();
			}
		}

		[Script]
		public class Animation : ISupportsContainer, IDisposable
		{
			public readonly AnimationInfo Info;

			public Image[] Frames;

			public Canvas Container { get; set; }


			public Animation(AnimationInfo Info)
			{
				this.Info = Info;

				this.Container = new Canvas
				{
					Width = Info.Width,
					Height = Info.Height
				};

				this.Frames = Info.Frames.ToArray(
					source =>
						new Image
						{
							Source = source.ToSource(),
							Visibility = Visibility.Hidden
						}.AttachTo(this)
				);

				Action Hide = this.Frames.Last().ShowAndHideLater();

				this.Frames.AsCyclicEnumerable().ForEach(
					(Current, Next) =>
					{
						if (this.IsDisposed)
						{
							return;
						}

						Hide();
						Hide = Current.ShowAndHideLater();

						this.Info.Framerate.AtDelay(Next);
					}
				);
			}

			#region IDisposable Members

			bool IsDisposed;

			public void Dispose()
			{
				IsDisposed = true;

				this.OrphanizeContainer();
			}

			#endregion
		}


		public AnimationInfo this[string prefix]
		{
			get
			{
				return this.List.FirstOrDefault(k => k.Prefix == prefix);
			}
		}

		public void Add(int fps, int w, int h, string prefix)
		{
			Add(
				new AnimationInfo
				{
					Framerate = fps,
					Width = w,
					Height = h,
					Prefix = prefix
				}
			);
		}

		public readonly List<AnimationInfo> List = new List<AnimationInfo>();

		public void Add(AnimationInfo a)
		{
			a.Frames = PrefixToFrames(a.Prefix);

			List.Add(a);
		}

		#region IEnumerable<AnimationInfo> Members

		public IEnumerator<AnimationCollection.AnimationInfo> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return List.GetEnumerator();
		}

		#endregion
	}

}
