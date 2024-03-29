﻿using BetterConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Commands
{
	public class ShowCommand : Command
	{
		public override string Name => "show";

		public override string Discription => "Command for loading a demo";

		public override ArgumentDescriptor[] ArgDescriptors => new ArgumentDescriptor[] 
		{
			new ArgumentDescriptor("demoName", "The demo to load", isPositional:true)
		};

		protected override void OnExecute(Argument[] args)
		{
			var demoName = args[0];
			
			switch (demoName.Value)
			{
				case "ui":
					DemoStarter.StartUIDemo();
					break;

				default:
					throw new ArgumentException("The argument demoName was not a valid demo name");
			}


		}
	}
}
