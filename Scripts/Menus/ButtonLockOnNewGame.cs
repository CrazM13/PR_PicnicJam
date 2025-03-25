using Godot;
using System;

public partial class ButtonLockOnNewGame : BetterButton {

	public override void _Ready() {
		base._Ready();

		bool locked = GameManager.Instance.GetUnlockedLevelCount() == 0;

		this.Disabled = locked;

	}

}
