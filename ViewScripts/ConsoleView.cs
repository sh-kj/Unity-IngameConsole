using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using System.Text;
using UniRx.Triggers;

namespace radiants.IngameConsole
{
	public class ConsoleView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text Text;

		[SerializeField]
		private TMP_InputField Input;

		[SerializeField]
		private UnityEngine.UI.Button DisplayToggleButton;

		private bool Display
		{ get; set; } = true;

		[SerializeField]
		private GameObject[] DisplayObjects;

		private CompositeDisposable Disposables = new CompositeDisposable();

		private void Start()
		{
			Text.text = "";
			Console.LogUpdateAsObservable
				.Subscribe(_log => UpdateText(_log))
				.AddTo(Disposables);

			Input.OnSubmitAsObservable()
				.Subscribe(_ => OnSubmitInput(Input.text))
				.AddTo(Disposables);

			DisplayToggleButton.OnClickAsObservable()
				.Subscribe(_ => ToggleDisplay())
				.AddTo(Disposables);
		}


		private void OnDestroy()
		{
			Disposables.Dispose();
		}

		private void UpdateText(List<string> logs)
		{
			StringBuilder builder = new StringBuilder();
			foreach (var log in logs)
			{
				builder.Append(log);
				builder.Append("\n");
			}

			Text.SetText(builder);
		}

		private void OnSubmitInput(string input)
		{
			Console.Log(input);
			Input.text = "";
			Console.ExecuteCommand(input.Split(' '));
		}

		private void ToggleDisplay()
		{
			Display = !Display;
			foreach (var obj in DisplayObjects)
			{
				obj.SetActive(Display);
			}
		}
	}
}