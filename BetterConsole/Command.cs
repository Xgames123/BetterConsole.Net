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


		/// <summary>
		/// Finds which command is used in the args and then runs it
		/// </summary>
		/// <returns>The command that was run</returns>
		public static Command RunCommand(string[] args, Command helpCommand=null, params Command[] commands)
		{
			if (args.Length == 0)
			{
				if (helpCommand == null)
				{
					return null;
				}

				helpCommand.Execute(Array.Empty<Argument>());

				return helpCommand;
			}


			foreach (Command command in commands)
			{
				if (args[0] == command.Name)
				{
					string[] commandArgs = new string[args.Length-1];
					Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
					ExecuteCommandFromArgs(command, commandArgs);

					return command;
				}

			}
			throw new ArgumentException($"The command '{args[0]}' was not a valid command");

		}

		private static void ExecuteCommandFromArgs(Command command, string[] args)
		{
			List<Argument> Args = new List<Argument>();
			for (int i = 0; i < command.ArgDescriptors.Length; i++)
			{
				ArgumentDescriptor argDescriptor = command.ArgDescriptors[i];

				Argument foundArg = null;
				if (argDescriptor.IsPositional)
				{
					if (args.Length > i)
					{
						foundArg = Argument.Parse(args[i]);
					}

					if (foundArg != null && !foundArg.IsPositional)
					{
						foundArg = null;
					}

					if (foundArg == null)
					{
						if (argDescriptor.DefaultValue == "")
						{
							throw new CommandParserException($"Required argument '{argDescriptor.Name}' was not given");
						}

						Args.Add(new Argument(argDescriptor.Name, argDescriptor.DefaultValue));
					}
					else
					{

						Args.Add(foundArg);
						
					}
						

					continue;
				}

				foundArg = FindArgInStringArray(argDescriptor, args);

				if (foundArg == null)
				{
					if (argDescriptor.DefaultValue == "")
					{
						throw new CommandParserException($"Required argument '{argDescriptor.Name}' was not given");
					}

					Args.Add(new Argument(argDescriptor.Name, argDescriptor.DefaultValue));
				}
				else
				{
					Args.Add(foundArg);
				}

				




			}


			command.Execute(Args.ToArray());

		}


		private static Argument FindArgInStringArray(ArgumentDescriptor argDescriptor, string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{

				var arg = Argument.Parse(args[i]);
				

				if (arg != null && arg.Name == argDescriptor.Name)
				{
					return arg;
				}
				


			}

			return null;
		}


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


		public abstract void OnExecute(Argument[] args);



	}

}
