using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace AvalonPipeMania.Code.Labs
{
	[Script]
	public class ExtendedFieldTest : Canvas
	{
		public const int DefaultWidth = 600;
		public const int DefaultHeight = 600;

		public ExtendedFieldTest()
		{
			this.Width = DefaultWidth;
			this.Height = DefaultHeight;


			var f = new ExtendedField(6, 12, DefaultWidth, DefaultHeight);

			f.Container.AttachTo(this);

			f.Overlay.AttachTo(this);

			#region incoming stream of randomized pipes
			Func<IEnumerable<Func<SimplePipe>>> GetRandomizedBuildablePipes =
				delegate
				{
					Console.WriteLine("GetRandomizedBuildablePipes");
					return SimplePipe.BuildablePipes.Randomize();
				};

			IEnumerator<Func<SimplePipe>> RandomizedBuildablePipes =
				GetRandomizedBuildablePipes.AsCyclicEnumerable().GetEnumerator();
			#endregion

			var UseablePipes = new Queue<SimplePipe>();
 
			RandomizedBuildablePipes.Take(5).ForEach(
				Constructor =>
				{
					var a = Constructor();

					a.Container.AttachTo(this).MoveTo(8, 100 + Pipe.Size * UseablePipes.Count);
					UseablePipes.Enqueue(a);
				}
			);

			f.Field.Tiles.Click +=
				delegate
				{
					var FieldReady = UseablePipes.Dequeue();
					FieldReady.Container.Orphanize();

					var a = RandomizedBuildablePipes.Take()();
					a.Container.AttachTo(this);
					UseablePipes.Enqueue(a);

					#region update
					UseablePipes.ForEach(
						(Current, Index) =>
						{
							Current.Container.MoveTo(8, 100 + Pipe.Size * Index);
						}
					);
					#endregion


				};

		}
	}
}
