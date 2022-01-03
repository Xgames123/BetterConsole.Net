using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class Argument
	{
		public string Name;
		public string Value = "";
		public bool IsPositional = false;

		public Argument()
		{

		}

		public Argument(string name, string value, bool isPositional = false)
		{
			Name = name;
			Value = value;
			IsPositional = isPositional;
		}
		public Argument(string value)
		{
			Name = "";
			IsPositional = true;
			Value = value;
		}


		public static Argument Parse(string str)
		{

			string[] argSplit = str.Split(":");

			if (argSplit.Length == 1)
			{
				return new Argument(str);
			}

			if (argSplit.Length != 2)
			{
				return null;
			}

			string argName = argSplit[0];
			string argValue = argSplit[1];

			return new Argument(argName, argValue);
		}



		public override string ToString()
		{

			return $"{Name}:{Value}";
		}

	}
}
