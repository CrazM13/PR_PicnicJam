using Godot;
using System;

public partial class SpinningLevelObject : LevelObject {

	[Export] private float speed = 1;

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (IsPowered) this.Rotation += (float) delta * speed;

	}

}
