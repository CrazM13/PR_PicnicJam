using Godot;
using System;

public partial class ScoreStar : TextureRect {

	[Export] private int minScore;

	public override void _Ready() {
		base._Ready();

		this.SelfModulate = GameManager.Instance.GetLastLevelData().Score >= minScore ? Colors.White : Colors.Black;

	}

}
