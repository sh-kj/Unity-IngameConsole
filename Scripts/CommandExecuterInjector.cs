using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace radiants.IngameConsole
{
	//radiants.IngameConsole.Console class does not reference CommandExecuter, 
	//so you have to trigger ConsoleCommand's static constructor to Dependency Injection.
	//You should add this component to boot scene or some place.
	public class CommandExecuterInjector : MonoBehaviour
	{
		private void Awake()
		{
			ConsoleCommands.DoNothing();
		}
	}
}