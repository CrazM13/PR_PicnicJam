using Godot;
using System;

public partial class ButtonLockIfNoNextLevel : BetterTextButton {

	private bool shouldUnlock;

	public override void _Ready() {
		base._Ready();

		string path = GameManager.Instance.GetLastLevelData().NextLevelPath;

		if (string.IsNullOrEmpty(path)) {
			this.Text = "Next";
		}

	}

}
