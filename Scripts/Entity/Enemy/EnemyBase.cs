using Godot;
using System;

public partial class EnemyBase : CharacterBody2D {

	[Signal] public delegate void CollisionWithLevelEventHandler(KinematicCollision2D collision);
	[Signal] public delegate void CollisionWithPlayerEventHandler(KinematicCollision2D collision, SimplePlayerController player);

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		MoveAndSlide();

		for (int i = 0; i < GetSlideCollisionCount(); i++) {
			KinematicCollision2D collision = GetSlideCollision(i);

			if (((Node) collision.GetCollider()).GetParent() is SimplePlayerController player) {
				EmitSignal(SignalName.CollisionWithPlayer, collision, player);
			} else {
				EmitSignal(SignalName.CollisionWithLevel, collision);
			}

		}

	}

}
