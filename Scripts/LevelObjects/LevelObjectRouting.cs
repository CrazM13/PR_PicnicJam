using Godot;
using System;

public partial class LevelObjectRouting : LevelObject {

	[Export] private LevelObject[] levelObjects;

	public override void _Ready() {

		this.PowerChange += this.OnPowerChange;

		base._Ready();
	}

	private void OnPowerChange(LevelObject _) {
		foreach (LevelObject lo in levelObjects) {
			lo.SetPower(IsPowered, ID);
		}

		for (int i = 0; i < GetChildCount(); i++) {
			if (GetChild(i) is LevelObject lo) {
				lo.SetPower(IsPowered, ID);
			}
		}

		QueueRedraw();
	}

	public override void _Draw() {
		base._Draw();

		foreach (LevelObject lo in levelObjects) {
			if (IsPowered) {
				DrawLine(Vector2.Zero, ToLocal(lo.GlobalPosition), Colors.Green);
			} else {
				DrawDashedLine(Vector2.Zero, ToLocal(lo.GlobalPosition), Colors.Red, -1, 8);
			}
		}

	}

}
