using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using ScriptCoreLib.Shared.Avalon.Extensions;
using ScriptCoreLib.Shared.Lambda;
using System.Windows.Controls;
using System.Windows.Media;
using ScriptCoreLib.Shared.Avalon.Tween;

namespace AvalonPipeMania.Code
{
	public partial class SimplePipeFeeder : IEnumerator<SimplePipe>
	{
		public readonly Canvas Container;

		public SimplePipe Current { get; set; }

		public readonly Action MoveNext;

		
		public SimplePipeFeeder(int VisiblePipes, Color DefaultPipeColor)
		{
			var AlternativeOffset = 8;

			Container = new Canvas
			{
				Width = Pipe.Size,
				Height = (Pipe.Size + AlternativeOffset) * VisiblePipes
			};

			#region incoming stream of randomized pipes
			Func<IEnumerable<Func<SimplePipe>>> GetRandomizedBuildablePipes =
				delegate
				{
					return SimplePipe.BuildablePipes.Randomize();
				};

			IEnumerator<Func<SimplePipe>> RandomizedBuildablePipes =
				GetRandomizedBuildablePipes.AsCyclicEnumerable().GetEnumerator();
			#endregion

			var UseablePipes = new Queue<SimplePipeWithEmitter>();

			Action<SimplePipeWithEmitter, int> MoveTo =
				(Current, Index) =>
					Current.MoveTo(0, (Pipe.Size + AlternativeOffset) * Index); 


			#region UseablePipesUpdate
			Action UseablePipesUpdate =
				delegate
				{
					UseablePipes.ForEach(
						(Current, Index) =>
						{
							Current.Pipe.Container.Opacity = Convert.ToDouble(VisiblePipes - Index) / VisiblePipes;

							MoveTo(Current, Index);
						}
					);
				};
			#endregion


			this.Current = RandomizedBuildablePipes.Take()();
			this.Current.Color = DefaultPipeColor;

			RandomizedBuildablePipes.Take(VisiblePipes).ForEach(
				Constructor =>
				{
					var a = new SimplePipeWithEmitter(Constructor());

					a.Pipe.Color = DefaultPipeColor;
 
					a.Pipe.Container.AttachTo(Container);
					UseablePipes.Enqueue(a);
				}
			);

			UseablePipesUpdate();

			this.MoveNext =
				delegate
				{
					var k = UseablePipes.Dequeue();

					k.Dispose();

					this.Current = k.Pipe;
					this.Current.Container.Orphanize();

					var a = new SimplePipeWithEmitter(RandomizedBuildablePipes.Take()());
					a.Pipe.Color = DefaultPipeColor;
					a.Pipe.Container.AttachTo(this.Container);
					UseablePipes.Enqueue(a);

					MoveTo(a, UseablePipes.Count);

					UseablePipesUpdate();
				};

		}

		[Script]
		public class SimplePipeWithEmitter : IDisposable
		{
			public SimplePipeWithEmitter(SimplePipe Pipe)
			{
				this.Pipe = Pipe;
				this.MoveTo = 
					NumericEmitter.Of(
						(x, y) =>
						{
							if (IsDisposed)
								return;

							Pipe.Container.MoveTo(x, y);
						}
					);
			}

			public readonly SimplePipe Pipe;
			public readonly Action<int, int> MoveTo;

			#region IDisposable Members

			public bool IsDisposed { get;  set; }

			public void Dispose()
			{
				IsDisposed = true;
			}

			#endregion
		}

	

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerator Members

		object System.Collections.IEnumerator.Current
		{
			get { return this.Current; }
		}

		bool System.Collections.IEnumerator.MoveNext()
		{
			this.MoveNext();

			return true;
		}

		void System.Collections.IEnumerator.Reset()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
