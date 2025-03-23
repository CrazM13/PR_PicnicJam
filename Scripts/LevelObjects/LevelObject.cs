using Godot;
using System;

public partial class LevelObject : Node2D {

	[Signal] public delegate void PowerChangeEventHandler(LevelObject levelObject);

	[Export] private bool invertPower = false;
	[Export] private bool toggleMode = false;

	public bool IsPowered { get; private set; }

	public override void _Ready() {
		base._Ready();

		if (invertPower) IsPowered = true;

	}

	public void SetPower(bool powered) {
		bool oldPower = IsPowered;

		if (toggleMode && ((invertPower && !powered) || (!invertPower && powered))) {
			IsPowered = !IsPowered;
		} else if (invertPower) {
			IsPowered = !powered;
		} else {
			IsPowered = powered;
		}

		if (oldPower != IsPowered) {
			EmitSignal(SignalName.PowerChange, this);
		}
	}

}
