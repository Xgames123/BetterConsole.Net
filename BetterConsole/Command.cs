using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{

	public abstract class Command
	{
		public abstract string Name { get; }
		public abstract string Discription { get; }

		public abstract ArgumentDescriptor[] ArgDescriptors { get; }


	

	
		public virtual string ToHelpString()
		{
			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append(Name);
			stringBuilder.Append(" ");
			foreach (var argDescriptor in ArgDescriptors)
			{
				if (argDescriptor.IsPositional)
				{
					stringBuilder.Append(argDescriptor.ToString());
					stringBuilder.Append(" ");
					continue;
				}
				stringBuilder.Append("{");
				stringBuilder.Append(argDescriptor.ToString());
				stringBuilder.Append("} ");
			}
			
			foreach (var argDescriptor in ArgDescriptors)
			{
				stringBuilder.Append("\n   ");
				stringBuilder.Append(argDescriptor.Name);
				stringBuilder.Append(": ");
				stringBuilder.Append(argDescriptor.Description);
				
			}

			return stringBuilder.ToString();
		}


		public void Execute(Argument[] args)
		{
			OnExecute(args);
		}


		protected abstract void OnExecute(Argument[] args);



	}

}
