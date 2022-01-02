using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class CommandlineArgument
	{
		public string Name;
		public string Value = "";


		public CommandlineArgument()
		{

		}

		public CommandlineArgument(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public static CommandlineArgument Parse(string str)
		{
			string[] argSplit = str.Split(":");
			if (argSplit.Length != 2)
			{
				return null;
			}

			string argName = argSplit[0];
			string argValue = argSplit[1];

			return new CommandlineArgument(argName, argValue);
		}



		public override string ToString()
		{

			return $"{Name}:{Value}";
		}

	}
}
