using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BetterConsole.UI;
using BetterConsole;

namespace Demo
{
	public static class DemoStarter
	{
		private static int MenuSize = 30;
		private static string DummyTextInput = "This is editable text";

		public static void StartUIDemo()
		{
			while (true)
			{
				var answer = ConsoleDrawer.DrawMenu(MenuSize, "This is a menu", CancellationToken.None, "Settings", "Option 1", "Option 2", "Help screen", "Exit");
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
						new DefaultHelpCommand(Program.Commands).Execute(Array.Empty<Argument>());
						break;
					case 4:
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
