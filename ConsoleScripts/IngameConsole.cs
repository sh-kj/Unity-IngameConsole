using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace radiants.IngameConsole
{
	public static class Console
	{
		private const int MaxLogNumber = 100;
		private static List<string> LogList = new List<string>();

		private static Subject<List<string>> LogUpdateSubject = new Subject<List<string>>();
		public static IObservable<List<string>> LogUpdateAsObservable = LogUpdateSubject;

		private static ICommandExecuter CommandExecuter = null;

		public static void Log(object o)
		{
			if (o != null)
				Log(o.ToString());
			else
				Log("null");
		}
		public static void Log(string text)
		{
			LogList.Add(text);

			while (LogList.Count > MaxLogNumber)
			{ LogList.RemoveAt(0); }

			LogUpdateSubject.OnNext(LogList);
		}

		public static void SetCommandExecuter(ICommandExecuter exec)
		{
			CommandExecuter = exec;
		}

		public static void ExecuteCommand(string[] command)
		{
			CommandExecuter?.Execute(command);
		}
	}

	public interface ICommandExecuter
	{
		void Execute(string[] command);
	}
}