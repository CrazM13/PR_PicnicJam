using Godot;
using System;

public partial class PhysicsObject : AnimatableBody2D {

	[Signal] public delegate void CollisionEventHandler(KinematicCollision2D collision, Vector2 velocity);
	[Signal] public delegate void PostCollisionEventHandler(KinematicCollision2D collision, Vector2 velocity);

	public Vector2 Velocity { get; set; }

	[Export] private float gravityScale = 1f;
	[Export] public float Mass { get; set; } = 1f;

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		Velocity *= 1 - this.PhysicsMaterialOverride.Friction;

		Velocity += this.GetGravity() * (float) delta * gravityScale;

		KinematicCollision2D collision = this.MoveAndCollide(Velocity * (float) delta);
		if (collision != null) {
			EmitSignal(SignalName.Collision, collision, Velocity);

			if (collision.GetCollider() is PhysicsObject otherBody) {
				otherBody.Velocity += Velocity * otherBody.Mass;
			}
			Velocity = this.PhysicsMaterialOverride.Bounce * Velocity.Bounce(collision.GetNormal());

			EmitSignal(SignalName.PostCollision, collision, Velocity);
		}

	}

	public void AddForce(Vector2 force) {
		Velocity += force / Mass;
	}

}
