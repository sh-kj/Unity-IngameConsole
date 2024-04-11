using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

namespace radiants.IngameConsole
{
	public class CommandExecuter
	{
		private Dictionary<string, MethodInfo> CommandDictionary = new Dictionary<string, MethodInfo>();

		public CommandExecuter(System.Type typeOfCommands)
		{
			//make command list using reflection
			MethodInfo[] methods = typeOfCommands.GetMethods()
				.Where(_method =>
				{
					//only static Action(string[]) are command
					if (!_method.IsStatic) return false;
					var parameters = _method.GetParameters();
					if (parameters.Length != 1) return false;
					if (_method.ReturnType != typeof(void)) return false;
					return parameters[0].ParameterType == typeof(string[]);
				}).ToArray();

			//Attribute is prior
			foreach (var method in methods)
			{
				var attributes = method.GetCustomAttributes<CommandNameAttribute>();
				foreach (var attr in attributes)
				{
					if(CommandDictionary.ContainsKey(attr.Name))
					{
						Debug.LogWarning("Duplicate command define:" + attr.Name);
						continue;
					}

					CommandDictionary.Add(attr.Name, method);
				}
			}

			//Method name is also available as command
			foreach (var method in methods)
			{
				if(!CommandDictionary.ContainsKey(method.Name))
					CommandDictionary.Add(method.Name, method);
			}
		}

		public void Execute(string[] args)
		{
			if (args.Length == 0) return;

			if(!CommandDictionary.ContainsKey(args[0]))
			{
				Console.Log("No such command:" + args[0]);
				return;
			}

			CommandDictionary[args[0]].Invoke(null, new object[] { args });
		}

		public IEnumerable<string> GetCommands()
		{
			return CommandDictionary.Keys;
		}
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandNameAttribute: Attribute
	{
		public string Name;
		public CommandNameAttribute(string name)
		{ Name = name; }
	}
}
