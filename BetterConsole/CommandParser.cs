using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole
{
	/// <summary>
	/// Class for parsing strings to commands
	/// </summary>
	public static class CommandParser
	{

		/// <summary>
		/// Finds which command is used and returns it and its arguments
		/// </summary>
		/// <param name="args"></param>
		/// <param name="commands"></param>
		/// <returns></returns>
		public static (Command, Argument[]) Parse(string[] args, params Command[] commands)
		{
			if (args.Length == 0)
			{
				return (null, Array.Empty<Argument>());
			}

			foreach (Command command in commands)
			{
				if (args[0] == command.Name)
				{
					string[] commandArgs = new string[args.Length - 1];
					Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
					ExecuteCommandFromArgs(command, commandArgs);

					return command;
				}

			}

		}




		/// <summary>
		/// Finds which command is used in and then runs it
		/// </summary>
		/// <returns>The command that was run</returns>
		public static Command ParseAndRun(string args, Command helpCommand=null, params Command[] commands)
		{
			return ParseAndRun(args.Split(" "), helpCommand, commands);
		}


		/// <summary>
		/// Finds which command is used in and then runs it
		/// </summary>
		/// <returns>The command that was run</returns>
		public static Command ParseAndRun(string[] args, Command helpCommand = null, params Command[] commands)
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
					string[] commandArgs = new string[args.Length - 1];
					Array.Copy(args, 1, commandArgs, 0, commandArgs.Length);
					ExecuteCommandFromArgs(command, commandArgs);

					return command;
				}

			}
			throw new CommandParserException($"The command '{args[0]}' was not a valid command");

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




	}
}
