using Godot;
using System;

public partial class ScoreStar : TextureRect {

	[Export] private int minScore;

	[Export] private SendStarToast starToast;
	[Export] private MoveTowardNode movingStarEffect;

	public override void _Ready() {
		base._Ready();

		this.SelfModulate = GameManager.Instance.GetLastLevelData().Score >= minScore ? Colors.White : new Color(Colors.Black, 0);

	}

	public void AttemptCountUp() {
		bool shouldCount = GameManager.Instance.GetLastLevelData().Score >= minScore;

		if (shouldCount) starToast.CountUp();
	}

	public void AttemptSendStar() {
		bool shouldCount = GameManager.Instance.GetLastLevelData().Score >= minScore;

		if (shouldCount) movingStarEffect.CanMove = true;
	}

}
