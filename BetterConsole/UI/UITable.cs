using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole.UI
{
	internal struct UITable
	{
		public int StartX;
		public int StartY;

		public int Width;
		public int Height;

		public (string name, int size)[] Collumns;

		public UITable(int startx, int starty, int width, int height, (string name, int size)[] columns) : this()
		{
			StartX = startx;
			StartY = starty;
			Width = width;
			Height = height;
			Collumns = columns;
		}

	}
}
