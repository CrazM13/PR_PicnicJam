using Godot;
using System;

public partial class ButtonLockIfNoNextLevel : BetterButton {

	private bool shouldUnlock;

	public override void _Ready() {
		base._Ready();

		string path = GameManager.Instance.GetLastLevelData().NextLevelPath;

		shouldUnlock = !string.IsNullOrEmpty(path);

	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (!this.Disabled && !shouldUnlock) this.Disabled = true;

	}

}
