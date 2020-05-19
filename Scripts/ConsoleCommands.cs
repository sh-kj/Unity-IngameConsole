using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace radiants.IngameConsole
{
	//You can add command methods for this class.
	//or, create partial class file to not edit this file/directory.
	public static partial class ConsoleCommands
	{
		//↓DO NOT REMOVE THIS: set executer to console by static constructor
		private static CommandExecuter Executer;
		static ConsoleCommands()
		{
			Executer = new CommandExecuter();
			Console.SetCommandExecuter(Executer);
		}
		public static void DoNothing()
		{ }
		//↑DO NOT REMOVE THIS


		//enumrate all commands
		[CommandName("list")]
		public static void List(string[] _)
		{
			var commands = Executer.GetAllCommands();
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (var command in commands)
			{
				stringBuilder.Append(command);
				stringBuilder.Append(" ");
			}

			Console.Log(stringBuilder.ToString());
		}


		//Sample: If you type "some-command" or "SomeCommand" at console,
		//this method will be invoke.
		[CommandName("some-command")]
		public static void SomeCommand(string[] _)
		{
			Console.Log("Sample");
		}

	}
}