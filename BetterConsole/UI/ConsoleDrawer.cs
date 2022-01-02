using System;
using System.Collections.Generic;
using System.Threading;

namespace BetterConsole.UI
{
	/// <summary>
	/// A class used for drawing UI to the console
	/// </summary>
	public static class ConsoleDrawer
	{

		private static Stack<int> LabelLengthStack = new Stack<int>();

		/// <summary>
		/// Draws a label to the console and removes it when you call PopLabel
		/// </summary>
		public static void PushLabel(string label)
		{
			LabelLengthStack.Push(label.Length);
			Console.WriteLine(label);
		}
		/// <summary>
		/// Removes the last label drawn using PushLabel()
		/// </summary>
		public static void PopLabel()
		{
			int labelLength = LabelLengthStack.Pop();

			(int x, int y) = Console.GetCursorPosition();

			Console.SetCursorPosition(x, y - 1);

			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < labelLength; i++)
			{
				Console.Write(" ");
			}
			Console.SetCursorPosition(x, y);

		}


		/// <summary>
		/// Draw a yes or no dialog
		/// </summary>
		/// <param name="size">Width of the dialog</param>
		/// <returns>false if the user selected no or if canceled else it returns true</returns>
		public static bool DrawYesOrNo(int size, string title, CancellationToken cancellationToken)
		{
			switch (DrawMenu(size, title, cancellationToken, "No", "Yes"))
			{
				case -1:
					return false;
				case 0:
					return false;
				case 1:
					return true;
			}
			return false;

		}

		/// <summary>
		/// Draws a menu to the console and returns the index of the item the user picked or -1 when canceled
		/// </summary>
		/// <param name="size">Width of menu</param>
		/// <returns></returns>
		public static int DrawMenu(int size, string title, CancellationToken cancellationToken, params string[] options)
		{
			int selectedOption = 0;

			(int startX, int startY) = Console.GetCursorPosition();

			int headerSize = 2;
			while (true)
			{
				string[] titlesplit = title.Split('\n');
				DrawHorizontalLine(size, '╔', '═', '╗');
				foreach (var line in titlesplit)
				{
					Console.Write('║');
					DrawTextCenterd(line, size);
					Console.Write("║\n");
					headerSize++;
				}
				DrawHorizontalLine(size, '╠', '═', '╣');



				for (int i = 0; i < options.Length; i++)
				{

					Console.Write("║");

					if (i == selectedOption)
					{
						Console.ForegroundColor = ConsoleColor.Black;
						Console.BackgroundColor = ConsoleColor.White;
					}
					DrawTextCenterd(options[i], size);

					Console.ResetColor();

					Console.Write("║\n");

				}

				DrawHorizontalLine(size, '╚', '═', '╝');

				while (true)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						ClearRect(startX, startY, size + 2, options.Length + headerSize + 2);
						Console.SetCursorPosition(startX, startY);
						return -1;
					}

					if (Console.KeyAvailable)
					{
						break;
					}
					Thread.Sleep(100);
				}


				var key = Console.ReadKey(true);
				if (key.Key == ConsoleKey.UpArrow)
				{
					if (selectedOption > 0)
					{
						selectedOption--;
					}

				}
				else if (key.Key == ConsoleKey.DownArrow)
				{
					if (selectedOption < options.Length - 1)
					{
						selectedOption++;
					}
				}
				else if (key.Key == ConsoleKey.Enter)
				{
					ClearRect(startX, startY, size + 2, options.Length + headerSize + 2);
					Console.SetCursorPosition(startX, startY);
					return selectedOption;
				}

				Console.SetCursorPosition(startX, startY);
			}



		}

		/// <summary>
		/// Clears a rectangle of the console
		/// </summary>
		public static void ClearRect(int x, int y, int w, int h)
		{
			(int startX, int StartY) = Console.GetCursorPosition();
			Console.SetCursorPosition(x, y);
			Console.ForegroundColor = ConsoleColor.Black;

			for (int i = 0; i < h; i++)
			{
				for (int ii = 0; ii < w; ii++)
				{
					Console.Write(" ");
				}
				Console.Write("\n");
			}
			Console.ResetColor();
			Console.SetCursorPosition(startX, StartY);

		}

		/// <summary>
		/// Draws a horizontal line
		/// </summary>
		/// <param name="length">Length of the line whiteout the start end and character</param>
		/// <param name="startChar">The character to draw before the line starts</param>
		/// <param name="lineChar">The character to draw when drawing the line</param>
		/// <param name="EndChar">The character to draw after the line</param>
		public static void DrawHorizontalLine(int length, char startChar, char lineChar, char EndChar)
		{
			Console.Write(startChar);
			for (int i = 0; i < length; i++)
			{
				Console.Write(lineChar);
			}
			Console.Write(EndChar);
			Console.Write('\n');
		}

		/// <summary>
		/// Draws centered text
		/// </summary>
		/// <param name="lineLength">The length of the line used to center the text</param>
		public static void DrawTextCenterd(string text, int lineLength)
		{
			int leftSize = lineLength / 2 - (text.Length / 2);
			int rightSize = lineLength - leftSize - text.Length;


			for (int ii = 0; ii < leftSize; ii++)
			{
				Console.Write(" ");
			}
			Console.Write(text);
			for (int ii = 0; ii < rightSize; ii++)
			{
				Console.Write(" ");
			}

		}

		/// <summary>
		/// Draws text that the user can edit and returns the modified text
		/// </summary>
		/// <param name="startText">the text to start</param>
		/// <returns>The text that the user modified</returns>
		public static string DrawTextInput(string startText, CancellationToken cancellationToken)
		{
			(int startX, int startY) = Console.GetCursorPosition();

			int cursorPos = 0;
			List<char> chars = new List<char>();
			chars.AddRange(startText.ToCharArray());
			while (true)
			{

				Console.SetCursorPosition(startX, startY);
				ClearRect(startX, startY, chars.Count + 2, 1);

				for (int i = 0; i < chars.Count; i++)
				{
					if (i == cursorPos)
					{
						Console.BackgroundColor = ConsoleColor.White;
						Console.ForegroundColor = ConsoleColor.Black;
					}
					Console.Write(chars[i]);
					Console.ResetColor();
				}
				if (cursorPos == chars.Count)
				{
					Console.BackgroundColor = ConsoleColor.White;
					Console.ForegroundColor = ConsoleColor.Black;
					Console.Write(' ');
					Console.ResetColor();
				}



				while (true)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						ClearRect(startX, startY, chars.Count, 1);
						Console.SetCursorPosition(startX, startY);
						return new string(chars.ToArray());
					}

					if (Console.KeyAvailable)
					{
						break;
					}
					Thread.Sleep(100);
				}
				var keyInfo = Console.ReadKey(true);
				if (keyInfo.Key == ConsoleKey.Backspace && chars.Count > 0 && cursorPos > 0)
				{
					cursorPos--;
					chars.RemoveAt(cursorPos);

				}
				else if (keyInfo.Key == ConsoleKey.Enter)
				{
					ClearRect(startX, startY, chars.Count, 1);
					Console.SetCursorPosition(startX, startY);
					return new string(chars.ToArray());
				}
				else if (keyInfo.Key == ConsoleKey.LeftArrow && cursorPos > 0)
				{
					cursorPos--;
				}
				else if (keyInfo.Key == ConsoleKey.RightArrow && cursorPos < chars.Count)
				{
					cursorPos++;
				}
				else if (!char.IsControl(keyInfo.KeyChar))
				{
					chars.Insert(cursorPos, keyInfo.KeyChar);
					cursorPos++;
				}


			}

		}

	}
}
