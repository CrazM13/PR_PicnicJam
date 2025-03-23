using Godot;
using System;

public partial class LevelObjectRotator : LevelObject {

	[Export] private float speed = 1;
	[Export] private float unpoweredRotation = 0;
	[Export] private float poweredRotation = 90;

	private float targetRotation;

	public override void _Ready() {
		this.PowerChange += this.OnPowerChange;
		base._Ready();
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		this.RotationDegrees = Mathf.MoveToward(this.RotationDegrees, targetRotation, (float) delta * speed);

	}

	private void OnPowerChange(LevelObject _) {
		targetRotation += IsPowered ? poweredRotation : unpoweredRotation;

		if (targetRotation > 360) {
			targetRotation -= 360;
		} else if (targetRotation < -360) {
			targetRotation += 360;
		}
	}
}
