using Godot;
using System;

public partial class LevelObjectButton : Area2D {

	[Export] private LevelObjectRouting router;
	[Export] private Sprite2D sprite;

	int level = 0;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
		this.BodyExited += this.OnBodyExit;
	}

	private void OnBodyExit(Node2D body) {
		level--;

		if (level == 0) {
			router.SetPower(false);
			sprite.Offset = Vector2.Zero;
		}
	}

	private void OnBodyEnter(Node2D body) {
		level++;

		router.SetPower(true);
		sprite.Offset = sprite.Offset = new Vector2(0, 64);
	}

}
