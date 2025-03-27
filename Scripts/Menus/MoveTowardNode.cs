using Godot;
using System;

public partial class MoveTowardNode : Control {

	[Export] private Control target;
	[Export] private float speed;

	[Export] public bool CanMove { get; set; } = true;

	public override void _Process(double delta) {
		base._Process(delta);

		if (CanMove) this.GlobalPosition = this.GlobalPosition.MoveToward(target.GlobalPosition, (float) delta * speed);

	}

}
