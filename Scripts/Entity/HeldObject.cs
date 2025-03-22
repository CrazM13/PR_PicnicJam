using Godot;
using System;

public partial class HeldObject : CharacterBody2D {

	public bool IsCollectable { get; set; } = true;

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Velocity += GetGravity() * (float) delta;

		MoveAndSlide();

	}

}
