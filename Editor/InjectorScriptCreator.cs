using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#nullable enable

namespace radiants.IngameConsole
{
	public static class InjectorScriptCreatorWindowCaller
	{
		//Check CommandInjector script exists,
		//and open window automatically when it doesn't exist
		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded()
		{
			//Debug.Log("OnScriptsReloaded");
			string? injectorPath = InjectorScriptCreator.CheckInjectorScriptExists();
			//Debug.Log("check:" + injectorPath);
			//CommandInjector is already exists
			if (injectorPath != null)
				return;

			OpenCommandInjectorWindow();
		}




		[MenuItem("Window/IngameConsole/Create Command Injector")]
		private static void OpenCommandInjectorWindow()
		{
			var window = EditorWindow.GetWindow<InjectorScriptCreator>(true, "Create Command Injector", true);
			window.minSize = new Vector2(500, 200);
			window.maxSize = new Vector2(500, 200);
			window.ShowPopup();
		}
	}


	public class InjectorScriptCreator : EditorWindow
	{
		public static string? CheckInjectorScriptExists()
		{
			return CheckScriptExists("CommandInjector");
		}
		public static string? CheckCommandsScriptExists()
		{
			return CheckScriptExists("ConsoleCommands");
		}
		private static string? CheckScriptExists(string fileName)
		{
			var found = AssetDatabase.FindAssets("t:Script " + fileName);
			//Debug.Log(found.Length);

			if (found.Length > 0)
			{
				return AssetDatabase.GUIDToAssetPath(found[0]);
			}
			return null;
		}


		private void OnGUI()
		{
			string? injectorPath = CheckInjectorScriptExists();

			if (injectorPath != null)
			{
				DrawInjectorExists(injectorPath);
				return;
			}
			else
			{
				DrawInjectorIsNotExist();
				return;
			}
		}

		private void DrawInjectorExists(string injectorPath)
		{
			GUILayout.Label("CommandInjector script already exists in:\n" + injectorPath + "\nAttach it to boot scene to use console commands.");
			GUILayout.Space(10f);
			string? commandsPath = CheckCommandsScriptExists();
			if (commandsPath != null)
			{
				GUILayout.Label("ConsoleCommands script already exists in:\n" + commandsPath + "\n" +
					"You can edit it, or add script file by using Partial class to create console commands.");
			}
			else
			{
				GUILayout.Label("ConsoleCommands script is not found.");
				if(GUILayout.Button("Create"))
				{
					CreateScript("ConsoleCommands");
				}
			}

			GUILayout.Space(10f);
			GUILayout.Label("CommandInjector.cs and ConsoleCommands.cs can move any directory under Assets/, \nbut we recommend not to move under any asmdefs.");
		}
		private void DrawInjectorIsNotExist()
		{
			GUILayout.Label("You have to create CommandInjector script, \nand attach it to boot scene to use console commands.");
			if (GUILayout.Button("Create"))
			{
				CreateScript("CommandInjector");
			}
		}

		private void CreateScript(string scriptName)
		{
			var origin = GetScriptOriginText(scriptName);

			System.IO.File.WriteAllText("Assets/" + scriptName + ".cs", origin.text);
			AssetDatabase.Refresh();
		}

		private const string PackageManagerScriptTextPath = "Packages/com.radiants.ingameconsolesystem/Editor/";
		private static TextAsset GetScriptOriginText(string scriptName)
		{
			//first, search in package manager directory
			string textPath = PackageManagerScriptTextPath + scriptName + ".txt";

			TextAsset? origin = AssetDatabase.LoadAssetAtPath<TextAsset>(textPath);
			if(origin != null) return origin;

			//if not found, search this script's directory
			string? thisScriptPath = CheckScriptExists("InjectorScriptCreator");
			if (thisScriptPath == null) throw new System.Exception("Create " + scriptName + " script Failed.");

			string thisScriptDirectory = thisScriptPath.Substring(0, thisScriptPath.LastIndexOf('/') + 1);
			textPath = thisScriptDirectory + scriptName + ".txt";
			origin = AssetDatabase.LoadAssetAtPath<TextAsset>(textPath);
			if (origin != null) return origin;

			//not found: throw exception
			throw new System.Exception("Create " + scriptName + " script Failed.");
		}

	}
}