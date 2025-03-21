using Godot;
using System;

public partial class PlayerController : Node {

	[Export] private PhysicsSpring downSpring;
	[Export] private PhysicsSpring upSpring;
	[Export] private PhysicsSpring leftSpring;
	[Export] private PhysicsSpring rightSpring;

	float restLength;
	float extendLength;

	public override void _Ready() {
		base._Ready();

		restLength = downSpring.RestLength;
		extendLength = restLength * 10;

	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		downSpring.RestLength = Input.IsActionPressed("ui_down") ? extendLength : restLength;
		upSpring.RestLength = Input.IsActionPressed("ui_up") ? extendLength : restLength;
		leftSpring.RestLength = Input.IsActionPressed("ui_left") ? extendLength : restLength;
		rightSpring.RestLength = Input.IsActionPressed("ui_right") ? extendLength : restLength;

	}

}
