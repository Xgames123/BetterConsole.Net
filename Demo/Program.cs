using System;
using BetterConsole;
using BetterConsole.UI;

namespace Demo
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Command.RunCommand(args, new SayMessageCommand());


			Console.ReadKey();

		}
	}
}
