using BetterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
	public class SayMessageCommand : Command
	{
		public override string Name => "say";
		public override string Discription => "A demo command to print a message to the console";
		public override ComandlineArgumentDescriptor[] ArgDescriptors => new ComandlineArgumentDescriptor[] {new ComandlineArgumentDescriptor("message", "The message to print", "this is a demo") };

		public override void OnExecute(CommandlineArgument[] args)
		{
			var messageArg = args[0];

			Console.WriteLine(messageArg.Value);

		}
	}
}
