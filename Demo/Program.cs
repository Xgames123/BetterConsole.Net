using System;
using BetterConsole;
using Demo.Commands;

namespace Demo
{
	internal class Program
	{
		public static Command[] Commands = new Command[] { new SayMessageCommand(), new ShowCommand() };

		static void Main(string[] args)
		{

			Command.RunCommand(args, helpCommand:new DefaultHelpCommand(Commands), Commands);


			Console.ReadKey();

		}
	}
}
