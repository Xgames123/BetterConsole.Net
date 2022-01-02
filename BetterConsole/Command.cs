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

        public abstract ComandlineArgumentDescriptor[] ArgDescriptors { get; }


        /// <summary>
        /// Finds which command is used in the args and then runs it
        /// </summary>
        /// <returns>The command that was run</returns>
        public static Command RunCommand(string[] args, params Command[] commands)
        {
            if (args.Length == 0)
            {
                return null;
            }


            foreach (Command command in commands)
            {
                if (args[0] == command.Name)
                {
                    ExecuteCommandFromArgs(command, args);

                    return command;
                }

            }


            return null;
        }

        private static void ExecuteCommandFromArgs(Command command, string[] args)
		{
            List<CommandlineArgument> Args = new List<CommandlineArgument>();
			foreach (var argDescriptor in command.ArgDescriptors)
			{
                CommandlineArgument foundArg = FindArgInStringArray(argDescriptor, args);
                

                if (foundArg == null)
				{
                    if (argDescriptor.DefaultValue == "")
					{
                        throw new CommandParserException($"Required arg '{argDescriptor.Name}' was not given");
                    }

                    Args.Add(new CommandlineArgument(argDescriptor.Name, argDescriptor.DefaultValue));
                }
				else
				{
                    Args.Add(foundArg);
                }

                
            }


            command.Execute(Args.ToArray());

        }
        private static CommandlineArgument FindArgInStringArray(ComandlineArgumentDescriptor argDescriptor, string[] args)
		{
            for (int i = 0; i < args.Length; i++)
            {

                var arg = CommandlineArgument.Parse(args[i]);
                
                if (arg != null && arg.Name == argDescriptor.Name)
                {
                    return arg;
                }


            }

            return null;
        }


        public void Execute(CommandlineArgument[] args)
		{
            OnExecute(args);
		}


        public abstract void OnExecute(CommandlineArgument[] args);



    }

}
