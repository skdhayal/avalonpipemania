using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScriptCoreLib;
using System.Windows.Controls;
using ScriptCoreLib.Shared.Lambda;
using ScriptCoreLib.Shared.Avalon.Extensions;

namespace AvalonPipeMania.Code
{
	partial class SimplePipe
	{

		[Script]
		public class Group
		{
			public void RaiseLeft()
			{
				if (Left != null)
					Left();
			}


			public void RaiseTop()
			{
				if (Top != null)
					Top();
			}



			public void RaiseRight()
			{
				if (Right != null)
					Right();
			}


			public void RaiseBottom()
			{
				if (Bottom != null)
					Bottom();
			}

			public Action Left;
			public Action Top;
			public Action Right;
			public Action Bottom;

			public Action Pump;
			public Action Drain;
			public Action Spill;

			#region indexed
			public Action this[int x, int y]
			{
				get
				{
					if (x == 0)
					{
						if (y == -1)
							return this.Top;

						if (y == 1)
							return this.Bottom;


					}
					else if (y == 0)
					{
						if (x == -1)
							return this.Left;

						if (x == 1)
							return this.Right;
					}

					return null;
				}

				set
				{
					if (x == 0)
					{
						if (y == -1)
						{
							this.Top = value;
						}
						else if (y == 1)
						{
							this.Bottom = value;
						}
					}
					else if (y == 0)
					{
						if (x == -1)
						{
							this.Left = value;
						}
						else if (x == 1)
						{
							this.Right = value;

						}
					}

				}
			}
			#endregion

		}
	}
}
