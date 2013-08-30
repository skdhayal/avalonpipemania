using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using AvalonPipeMania.Assets.Shared;
using ScriptCoreLib.Shared.Avalon.Extensions;
using System.Windows;
using ScriptCoreLib.Shared.Lambda;

namespace AvalonPipeMania.Code
{
	public class Explosion
	{
		public const int Size = 64;

		public readonly Canvas Container;

		public static readonly string[] Sources;

		static Explosion()
		{
			Sources = KnownAssets.Default.FileNames.Where(k => k.StartsWith(KnownAssets.Path.Explosion)).OrderBy(k => k).ToArray();
		}

		public readonly Image[] Frames;

		public Explosion()
		{
			this.Container = new Canvas
			{
				Width = Size,
				Height = Size,
			};

			this.Frames = Sources.ToArray(
				k => new Image
				{
					Source = k.ToSource(),
					Visibility = Visibility.Hidden
				}.AttachTo(this.Container)
			);

		}

		public Explosion PlayAndOrphanize()
		{
			Play(
				() => this.Container.Orphanize()
			);

			return this;
		}

		public void Play(Action Done)
		{
			Frames.ForEach(
					(Current, Next) =>
					{
						Current.Show();

						(1000 / 23).AtDelay(
							delegate
							{
								Current.Hide();
								Next();
							}
						);
					}
				)(Done);
		}
	}
}
