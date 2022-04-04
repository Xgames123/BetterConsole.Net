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
				var answer = ConsoleDrawer.DrawMenu(MenuSize, "This is a menu", CancellationToken.None, "Settings", "Push Label", "Pop Label", "Option 1", "Option 2", "Exit");
				switch (answer)
				{
					case -1:
						return;
					case 0:
						DoSettings();
						break;
					case 1:
						ConsoleDrawer.PushLabel("this is a pushed label");
						break;
					case 2:
						ConsoleDrawer.PopLabel();
						break;
					case 3:
						Console.WriteLine("This is Option 1 printed using Console.WriteLine");
						break;
					case 4:
						Console.WriteLine("This is Option 2 printed using Console.WriteLine");
						break;
					case 5:
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
