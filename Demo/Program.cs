using System;
using BetterConsole;
using Demo.Commands;

namespace Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var commands = new Command[] { new SayMessageCommand(), new DemoCommand() };

			Command.RunCommand(args, helpCommand:new DefaultHelpCommand(commands), commands);


			Console.ReadKey();

		}
	}
}
