using Godot;
using System;

public partial class BirdController : Area2D {

	[Export] private EnemyBase enemy;

	[Export] private float speed = 1000;

	private SimplePlayerController player;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
		this.BodyExited += this.OnBodyExit;

		enemy.CollisionWithLevel += this.OnCollideWithLevel;
		enemy.CollisionWithPlayer += this.OnCollideWithPlayer;

		enemy.Velocity += Vector2.Right * speed;
	}

	private void OnCollideWithPlayer(KinematicCollision2D collision, SimplePlayerController player) {
		enemy.Velocity += collision.GetNormal() * speed;
		player.Basket.EjectContents();
		this.player = null;
	}

	private void OnCollideWithLevel(KinematicCollision2D collision) {
		enemy.Velocity += collision.GetNormal() * speed;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (player != null) {
			enemy.Velocity += enemy.GlobalPosition.DirectionTo(player.GlobalPosition) * speed;
		}

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
