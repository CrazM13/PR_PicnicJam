using Godot;
using System;

public partial class LevelObjectButton : Area2D {

	[Export] private LevelObjectRouting router;
	[Export] private Sprite2D sprite;
	[Export] private AudioStreamPlayer2D audioOn;
	[Export] private AudioStreamPlayer2D audioOff;

	int level = 0;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
		this.BodyExited += this.OnBodyExit;
	}

	private void OnBodyExit(Node2D body) {
		level--;

		if (level == 0) {
			audioOff.Play();
			router.SetPower(false, 0);
			sprite.Offset = Vector2.Zero;
		}
	}

	private void OnBodyEnter(Node2D body) {
		if (level <= 0) {
			audioOn.Play();
		}

		level++;

		router.SetPower(true, 0);
		sprite.Offset = sprite.Offset = new Vector2(0, 32);
	}

}
