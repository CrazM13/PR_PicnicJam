using Godot;
using System;

public partial class ButtonLockOnNewGame : BaseButton {

	public override void _Ready() {
		base._Ready();

		bool locked = GameManager.Instance.GetUnlockedLevelCount() == 0;

		this.Disabled = locked;

	}

}
