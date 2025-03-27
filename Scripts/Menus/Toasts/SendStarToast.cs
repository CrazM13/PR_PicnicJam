using Godot;
using System;
using System.ComponentModel;

public partial class SendStarToast : SendToast {

	Toast starToast;

	uint uncountedStars = 0;
	uint starsToCount = 0;

	public override Toast SendNewToast() {

		uncountedStars = GameManager.Instance.GetLastLevelStarGain();
		starsToCount = (uint) GameManager.Instance.GetLastLevelData().Score;

		starToast ??= Container.AddToast(5, string.Format(Message, (GameManager.Instance.Stats.StarCount - uncountedStars).ToString("D3")), Icon);

		return starToast;

	}

	public void CountUp() {
		if (starsToCount == uncountedStars) {
			uncountedStars--;
		}
		starsToCount--;
		starToast.SetMessage(5, string.Format(Message, (GameManager.Instance.Stats.StarCount - uncountedStars).ToString("D3")), Icon, false);
	}

}
