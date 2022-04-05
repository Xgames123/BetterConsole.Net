using System;
using System.Threading;
using BetterConsole.UI;
namespace Demo
{
	public class Program
	{

		private static int MenuSize = 80;
		private static string DummyTextInput = "This is editable text";


		static void Main(string[] args)
		{
			while (true)
			{
				ConsoleDrawer.PushLabel("hi this is a label");
				var usedSpace = (MenuSize / 4) + (MenuSize / 2);
				ConsoleDrawer.BeginTable(ConsoleColor.White, ("Name", MenuSize/4), ("Date", MenuSize / 2), ("Number", MenuSize - usedSpace));
				ConsoleDrawer.TableRow("name 0", "Tuesday, July 2, 2022", "0");
				ConsoleDrawer.TableRow("name 1", "Tuesday, July 20, 2022", "1000");
				ConsoleDrawer.TableRow("name 2", "Tuesday, June 3, 2019", "892703");
				ConsoleDrawer.TableRow("name 3", "Friday, June 3, 2019", "-30388");
				ConsoleDrawer.TableRow("name 4");
				ConsoleDrawer.EndTable();

				var answer = ConsoleDrawer.DrawMenu(MenuSize, "This is a menu", CancellationToken.None, "Settings", "Option 1", "Option 2", "Exit");

				ConsoleDrawer.PopAll();
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
