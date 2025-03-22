using Godot;
using System;

public partial class SpinningNode2D : Node2D {

	[Export] private float speed = 1;

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		this.Rotation += (float) delta * speed;

	}

}
