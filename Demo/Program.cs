using System;
using System.Threading;
using BetterConsole.UI;
namespace Demo
{
	public class Program
	{

		private static int MenuSize = 30;
		private static string DummyTextInput = "This is editable text";


		static void Main(string[] args)
		{
			while (true)
			{
				ConsoleDrawer.PushLabel("hi this is a label");

				ConsoleDrawer.BeginTable(50, "Ip Address", "Name", "Score");
				ConsoleDrawer.EndTable();

				var answer = ConsoleDrawer.DrawMenu(MenuSize, "This is a menu", CancellationToken.None, "Settings", "Option 1", "Option 2", "Exit");
	
				ConsoleDrawer.Pop(2);
				switch (answer)
				{
					case -1:
						return;
					case 0:
						DoSettings();
						break;
					case 1:
						Console.WriteLine("This is Option 1 printed using Console.WriteLine");
						break;
					case 2:
						Console.WriteLine("This is Option 2 printed using Console.WriteLine");
						break;
					case 3:
						return;

				}
			}

		}


		public static void DoSettings()
		{
			while (true)
			{
				var answer = ConsoleDrawer.DrawMenu(MenuSize, "Settings", CancellationToken.None, "Back", "MenuSize", "Text input");
				switch (answer)
				{
					case -1:
						return;
					case 0:
						return;
					case 1:
						MenuSize = ConsoleDrawer.DrawNumberInput(MenuSize, false);
						break;
					case 2:
						DummyTextInput = ConsoleDrawer.DrawTextInput(DummyTextInput, CancellationToken.None);
						break;

				}
			}


		}

	}
}
