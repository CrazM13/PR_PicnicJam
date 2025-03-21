using Godot;
using System;

public partial class PhysicsSpring : Node2D {

	[Export] private float stiffness = 1f; // K
	[Export] private float damping = 0.1f; // C
	[Export] private PhysicsObject source;
	[Export] private Vector2 sourceOffset1;
	[Export] private Vector2 sourceOffset2;
	[Export] private PhysicsObject attachment;
	[Export] private Vector2 attachmentOffset1;
	[Export] private Vector2 attachmentOffset2;

	[Export] public float RestLength { get; set; }

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		CalculateSpring(sourceOffset1, attachmentOffset1);
		CalculateSpring(sourceOffset2, attachmentOffset2);

		QueueRedraw();
	}

	public override void _Draw() {
		base._Draw();

		DrawSpring(sourceOffset1, attachmentOffset1);
		DrawSpring(sourceOffset2, attachmentOffset2);

	}

	private void CalculateSpring(Vector2 sourceOffset, Vector2 targetOffet) {
		Vector2 connectionPoint1 = source.GlobalPosition + sourceOffset;
		Vector2 connectionPoint2 = attachment.GlobalPosition + targetOffet;

		Vector2 direction = connectionPoint1.DirectionTo(connectionPoint2);

		float distance = connectionPoint2.DistanceTo(connectionPoint1);

		float retractionForce = stiffness * (distance - RestLength);

		source.Velocity += direction * retractionForce * (damping == 0 ? 1 : (1 / damping));
		attachment.Velocity -= direction * retractionForce * (damping == 0 ? 1 : (1 / damping));
	}

	private void DrawSpring(Vector2 sourceOffset, Vector2 targetOffet) {
		Vector2 connectionPoint1 = source.GlobalPosition + sourceOffset;
		Vector2 connectionPoint2 = attachment.GlobalPosition + targetOffet;

		DrawLine(ToLocal(connectionPoint1), ToLocal(connectionPoint2), Colors.Green);
	}

}
