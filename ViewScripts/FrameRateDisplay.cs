using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace radiants.IngameConsole
{
	public class FrameRateDisplay : MonoBehaviour
	{
		[SerializeField]
		private SpriteDigits.SpriteDigitsFloat Digits;

		[SerializeField]
		private float MeanSeconds = 0.2f;

		private int MeanFrameCount
		{ get; set; }
		private float TimeCount
		{ get; set; }

		private void Start()
		{
			MainThreadDispatcher.UpdateAsObservable()
				.Subscribe(_ => OnUpdate());
		}

		private void OnUpdate()
		{
			TimeCount += Time.unscaledDeltaTime;
			MeanFrameCount++;

			if (TimeCount < MeanSeconds) return;

			Digits.Value = MeanFrameCount / TimeCount;

			MeanFrameCount = 0;
			TimeCount = 0f;
		}


	}
}