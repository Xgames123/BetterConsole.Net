using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class ArgumentDescriptor
	{
		/// <summary>
		/// Name of the argument.
		/// </summary>
		public string Name="";
		/// <summary>
		/// Description to use for help
		/// </summary>
		public string Description;

		/// <summary>
		/// The default value for the argument if the argument is not defined.
		/// If DefaultValue is a empty string than it is required
		/// </summary>
		public string DefaultValue = "";

		/// <summary>
		/// If true the argument is positional
		/// </summary>
		public bool IsPositional = false;

		public ArgumentDescriptor(string name, string description, string defaultValue="", bool isPositional=false)
		{
			DefaultValue = defaultValue;
			Description = description;
			Name = name;
			IsPositional = isPositional;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(Name);
			if (DefaultValue == "")
			{
				return stringBuilder.ToString();
			}

			stringBuilder.Append(":");
			stringBuilder.Append(DefaultValue);
			

			return stringBuilder.ToString();


		}


	}
}
