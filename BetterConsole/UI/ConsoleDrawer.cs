using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BetterConsole.UI
{
	/// <summary>
	/// A class used for drawing UI to the console
	/// </summary>
	public static class ConsoleDrawer
	{

		private static Stack<UIStackEntry> _uiStack = new Stack<UIStackEntry>();

		private static UITable _currentTable = default;
		private static bool _beganTable = false;

		/// <summary>
		/// Starts drawing a table
		/// Call TableRow to add a row
		/// Call EndTable to end the table
		/// </summary>
		/// <param name="size">Width of the table</param>
		/// <param name="collumns">All collumns the table will contain</param>
		public static void BeginTable(int size, params string[] collumns)
		{
			(int startx, int starty) = Console.GetCursorPosition();

			int collumnSize = size / collumns.Length;

			//Draw top bar
			Console.Write('╔');
			for (int i = 0; i < collumns.Length; i++)
			{
				DrawHorizontalLine(collumnSize-2, '═', addNewlineOnEnd:false);
				Console.Write('╦');

			}
			MoveConsoleCursor(-1, 0);
			Console.Write('╗');
			Console.Write('\n');

			//Draw columns
			for (int i = 0; i < collumns.Length; i++)
			{
				Console.Write('║');
				DrawTextCenterd(collumns[i], collumnSize);
				
			}
			Console.Write('║');

			Console.Write('\n');
			//Draw bottom bar
			Console.Write('╠');
			for (int i = 0; i < collumns.Length; i++)
			{
				DrawHorizontalLine(collumnSize - 2, '═', addNewlineOnEnd: false);
				Console.Write('╬');

			}
			MoveConsoleCursor(-1, 0);
			Console.Write('╣');
			Console.Write('\n');

			_currentTable = new UITable(startx, starty, size, 3, collumns, size);
			_beganTable = true;
		}

		/// <summary>
		/// Adds a row to the current table
		/// </summary>
		/// <param name="values"></param>
		public static void TableRow(params string[] values)
		{

		}


		/// <summary>
		/// Ends drawing the table and pushes it to the UI stack
		/// </summary>
		public static void EndTable()
		{
			if (!_beganTable)
			{
				throw new InvalidOperationException("BeginTable has to be called before EndTable");
			}
			_beganTable = false;

			_currentTable.Height += 1;

			int collumnSize = _currentTable.Size / _currentTable.Collumns.Length;

			Console.Write('╚');
			for (int i = 0; i < _currentTable.Collumns.Length; i++)
			{
				DrawHorizontalLine(collumnSize - 2, '═', addNewlineOnEnd: false);
				Console.Write('╩');

			}
			MoveConsoleCursor(-1, 0);
			Console.Write('╝');
			Console.Write('\n');

			_uiStack.Push(new UIStackEntry(_currentTable.StartX, _currentTable.StartY, _currentTable.Width+3, _currentTable.Height));
		}



		/// <summary>
		/// Draws a label to the console and removes it when you call Pop
		/// </summary>
		public static void PushLabel(string label, ConsoleColor foregroundColor=ConsoleColor.White, ConsoleColor backgroundColor=ConsoleColor.Black)
		{
			Console.ForegroundColor = foregroundColor;
			Console.BackgroundColor = backgroundColor;

			_uiStack.Push(new UIStackEntry(label.Length, 1));
			Console.WriteLine(label);
			Console.ResetColor();
		}
		/// <summary>
		/// Draws a label to the console and removes it when you call Pop
		/// </summary>
		public static void PushVerticalSpaceing(int amount)
		{
			_uiStack.Push(new UIStackEntry(1, amount));
			for (int i = 0; i < amount; i++)
			{
				Console.Write('\n');
			}
			
		}

		/// <summary>
		/// Uses Console.SetCursorPosition to move the cursor to an offset
		/// </summary>
		/// <param name="offsetX">Amount to move in x</param>
		/// <param name="offsetY">Amount to move in y</param>
		public static void MoveConsoleCursor(int offsetX, int offsetY)
		{
			(int x, int y) = Console.GetCursorPosition();

			Console.SetCursorPosition(x+ offsetX, y+ offsetY);
		}



		/// <summary>
		/// Removes the last ui entry that was drawn using Push
		/// </summary>
		/// <param name="count">Amount of entries to pop</param>
		public static void Pop(int count=1)
		{
			for (int i = 0; i < count; i++)
			{
				var entry = _uiStack.Pop();
				entry.Clear();
			}
			
		}


		/// <summary>
		/// Draw a yes or no dialog
		/// </summary>
		/// <param name="size">Width of the dialog</param>
		/// <returns>false if the user selected no or if canceled else it returns true</returns>
		public static bool DrawYesOrNo(int size, string title, CancellationToken cancellationToken)
		{
			return DrawMenu(size, title, cancellationToken, "No", "Yes") switch
			{
				-1 => false,
				0 => false,
				1 => true,
				_ => false,
			};
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
		/// <param name="character">The character to draw when drawing the line</param>
		public static void DrawHorizontalLine(int length, char character, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black, bool addNewlineOnEnd=true)
		{
			DrawHorizontalLine(length, character, character, character, foregroundColor, backgroundColor, addNewlineOnEnd);
		}

		/// <summary>
		/// Draws a horizontal line
		/// </summary>
		/// <param name="length">Length of the line whiteout the start end and character</param>
		/// <param name="startChar">The character to draw before the line starts</param>
		/// <param name="lineChar">The character to draw when drawing the line</param>
		/// <param name="endChar">The character to draw after the line</param>
		public static void DrawHorizontalLine(int length, char startChar, char lineChar, char endChar, ConsoleColor foregroundColor=ConsoleColor.White, ConsoleColor backgroundColor=ConsoleColor.Black, bool addNewlineOnEnd = true)
		{
			Console.ForegroundColor = foregroundColor;
			Console.BackgroundColor = backgroundColor;

			Console.Write(startChar);
			for (int i = 0; i < length; i++)
			{
				Console.Write(lineChar);
			}
			Console.Write(endChar);
			if (addNewlineOnEnd)
			{
				Console.Write('\n');
			}
			Console.ResetColor();
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
		/// Draws a number that the user can edit and returns the modified number
		/// </summary>
		/// <returns>The number that the user modified</returns>
		public static int DrawNumberInput(int startNumber, bool canBeNegative=true, CancellationToken cancellationToken=default)
		{
			int outNum = -1;
			while (true)
			{
				string newNum = DrawTextInput(startNumber.ToString(), cancellationToken, true, charValidator: (char Char) => { return char.IsDigit(Char); });
				if (newNum == "")
				{
					return -1;
				}
				
				if (int.TryParse(newNum, out outNum))
				{
					if (canBeNegative)
					{
						break;
					}
					else
					{
						if (outNum > 0)
						{
							break;
						}

					}

					
				}
			}
			return outNum;


		}



		/// <summary>
		/// Draws text that the user can edit and returns the modified text
		/// </summary>
		/// <param name="charValidator">If the lambda returns true than the char is printed else it is skipped</param>
		/// <returns>The text that the user modified</returns>
		public static string DrawTextInput(string startText, CancellationToken cancellationToken, bool startCursorAtEnd=false, Func<char, bool> charValidator=null)
		{
			(int startX, int startY) = Console.GetCursorPosition();

			int cursorPos = 0;
			if (startCursorAtEnd)
			{
				cursorPos = startText.Length;
			}
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
						ClearRect(startX, startY, chars.Count+2, 1);
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
					ClearRect(startX, startY, chars.Count+2, 1);
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
					if (charValidator != null)
					{
						if (!charValidator.Invoke(keyInfo.KeyChar))
						{
							continue;
						}

					}

					chars.Insert(cursorPos, keyInfo.KeyChar);
					cursorPos++;
				}


			}

		}

	}
}
