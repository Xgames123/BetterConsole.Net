using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class DefaultHelpCommand : Command
	{
		public override string Name => "help";

		public override string Discription => "Shows a help screen";

		public override ArgumentDescriptor[] ArgDescriptors => Array.Empty<ArgumentDescriptor>();

		private Command[] Commands;


		public DefaultHelpCommand(params Command[] commands)
		{
			Commands = commands;
		}


		public override void OnExecute(Argument[] args)
		{
			foreach (var command in Commands)
			{
				Console.WriteLine(command.ToHelpString());
				Console.WriteLine();
			}


		}
	}
}
