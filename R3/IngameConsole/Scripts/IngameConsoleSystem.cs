using System.Collections.Generic;
using R3;

namespace radiants.IngameConsole
{
	public static class Console
	{
		private static int MaxLogNumber = 100;
		private static List<string> LogList = new List<string>();

		private static Subject<List<string>> LogUpdateSubject = new Subject<List<string>>();
		public static Observable<List<string>> LogUpdateAsObservable = LogUpdateSubject;

		private static CommandExecuter Executer = null;

		public static void Initialize(System.Type commandType)
		{
			Executer = new CommandExecuter(commandType);
		}


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

		public static void ExecuteCommand(string[] command)
		{
			Executer?.Execute(command);
		}

		public static IEnumerable<string> GetAllCommands()
		{
			return Executer?.GetCommands();
		}
	}
}