using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class CommandParserException : Exception
	{
		public CommandParserException() : base()
		{

		}

		public CommandParserException(string message) : base(message)
		{

		}
		public CommandParserException(string message, Exception inner) : base(message, inner)
		{

		}

	}
}
