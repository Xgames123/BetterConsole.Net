using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	public class ComandlineArgumentDescriptor
	{
		/// <summary>
		/// Name of the arg
		/// </summary>
		public string Name;
		/// <summary>
		/// Description to use for help
		/// </summary>
		public string Description;

		/// <summary>
		/// The default value for the arg if the arg is not defined.
		/// If DefaultValue is a empty string than it is required
		/// </summary>
		public string DefaultValue = "";

		public ComandlineArgumentDescriptor(string name, string description, string defaultValue="")
		{
			DefaultValue = defaultValue;
			Description = description;
			Name = name;
		}


	}
}
