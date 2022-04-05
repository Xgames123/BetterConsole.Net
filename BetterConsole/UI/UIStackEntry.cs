using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole.UI
{
	internal struct UIStackEntry
	{
		public int X;
		public int Y;
		public int Width;
		public int Height;
		

		public UIStackEntry(int x, int y, int width, int height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
			
		}
		public UIStackEntry(int width, int height)
		{
			(X, Y) = Console.GetCursorPosition();
			Width = width;
			Height = height;

		}

		/// <summary>
		/// Clears the space occupied by the UIStackEntry
		/// </summary>
		public void Clear()
		{
			(int startX, int startY) = Console.GetCursorPosition();
			ConsoleDrawer.ClearRect(X, Y, Width, Height);
			Console.SetCursorPosition(startX, startY-Height);

		}


	}
}
