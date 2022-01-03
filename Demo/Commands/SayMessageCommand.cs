using BetterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Commands
{
	public class SayMessageCommand : Command
	{
		public override string Name => "say";
		public override string Discription => "A demo command to print a message to the console";

		public override ArgumentDescriptor[] ArgDescriptors => new ArgumentDescriptor[] {
			new ArgumentDescriptor("message", "The message to print", isPositional:true),
			new ArgumentDescriptor("color", "The color of the message", "White")};

		public override void OnExecute(Argument[] args)
		{
			var messageArg = args[0];
			var colorArg = args[1];

			Console.ForegroundColor = Enum.Parse<ConsoleColor>(colorArg.Value, true);
			Console.WriteLine(messageArg.Value);
			Console.ResetColor();
		}
	}
}
