using Godot;
using System;

public partial class BirdController : Area2D {

	[Export] private float speed = 1000;

	[ExportGroup("References")]
	[Export] private EnemyBase enemy;
	[Export] private Node2D sprite;
	[ExportSubgroup("Audio")]
	[Export] private AudioStreamPlayer2D ambient;
	[Export] private AudioStreamPlayer2D attack;

	private SimplePlayerController player;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
		this.BodyExited += this.OnBodyExit;

		enemy.CollisionWithLevel += this.OnCollideWithLevel;
		enemy.CollisionWithPlayer += this.OnCollideWithPlayer;

		enemy.Velocity += Vector2.Right * speed;

		ambient.Play(((0.3f * this.GlobalPosition.X) + (0.7f * this.GlobalPosition.Y)) % 1);
	}

	private void OnCollideWithPlayer(KinematicCollision2D collision, SimplePlayerController player) {
		enemy.Velocity += collision.GetNormal() * speed;
		attack.Play();
		player.AttemptHurt();
		this.player = null;
	}

	private void OnCollideWithLevel(KinematicCollision2D collision) {
		enemy.Velocity += collision.GetNormal() * speed;
		this.player = null;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (player != null) {
			enemy.Velocity += enemy.GlobalPosition.DirectionTo(player.GlobalPosition) * speed;
		}

		sprite.Scale = new Vector2(Mathf.Abs(sprite.Scale.X) * (enemy.Velocity.X < 0 ? 1 : -1), sprite.Scale.Y);

	}


	private void OnBodyExit(Node2D body) {
		if (body is SimplePlayerController player) {
			this.player = null;
		}
	}

	private void OnBodyEnter(Node2D body) {
		if (body is SimplePlayerController player) {
			this.player = player;
		}
	}
}
