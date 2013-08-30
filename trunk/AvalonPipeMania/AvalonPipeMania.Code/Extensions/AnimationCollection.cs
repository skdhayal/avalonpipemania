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
    public class AnimationCollectionAnimationInfo
    {
        public int Framerate;
        public int Width;
        public int Height;
        public string Prefix;

        public string[] Frames;

        public AnimationCollection.Animation ToAnimation()
        {
            return new AnimationCollection.Animation(this);
        }

        public static implicit operator AnimationCollection.Animation(AnimationCollectionAnimationInfo e)
        {
            return e.ToAnimation();
        }
    }


	[Script]
	public class AnimationCollection : IEnumerable<AnimationCollectionAnimationInfo>
	{
		readonly Func<string, string[]> PrefixToFrames;

		public AnimationCollection(Func<string, string[]> PrefixToFrames)
		{
			this.PrefixToFrames = PrefixToFrames;
		}


		[Script]
		public class Animation : ISupportsContainer, IDisposable
		{
			public readonly AnimationCollectionAnimationInfo Info;

			public Image[] Frames;

			public Canvas Container { get; set; }


			public Animation(AnimationCollectionAnimationInfo Info)
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


		public AnimationCollectionAnimationInfo this[string prefix]
		{
			get
			{
				return this.List.FirstOrDefault(k => k.Prefix == prefix);
			}
		}

		public void Add(int fps, int w, int h, string prefix)
		{
			Add(
				new AnimationCollectionAnimationInfo
				{
					Framerate = fps,
					Width = w,
					Height = h,
					Prefix = prefix
				}
			);
		}

		public readonly List<AnimationCollectionAnimationInfo> List = new List<AnimationCollectionAnimationInfo>();

		public void Add(AnimationCollectionAnimationInfo a)
		{
			a.Frames = PrefixToFrames(a.Prefix);

			List.Add(a);
		}

		#region IEnumerable<AnimationInfo> Members

		public IEnumerator<AnimationCollectionAnimationInfo> GetEnumerator()
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
