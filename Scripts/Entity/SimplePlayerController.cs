using Godot;
using System;

public partial class SimplePlayerController : RigidBody2D {

	[Export] private float springCollisionForce = 1000f;
	[Export] private float springEmptyForce = 1000f;
	[Export] private float invincibilityTimer = 2.5f;

	[ExportGroup("References")]
	[Export] public HeldObjectArea Basket { get; private set; }

	[ExportSubgroup("Audio")]
	[Export] private AudioStreamPlayer2D springLaunch;
	[Export] private AudioStreamPlayer2D springBounce;

	[ExportSubgroup("Raycasts")]
	[Export] private RayCast2D upRay;
	[Export] private RayCast2D downRay;
	[Export] private RayCast2D leftRay;
	[Export] private RayCast2D rightRay;

	[ExportSubgroup("Colliders")]
	[Export] private CollisionShape2D upCollider;
	[Export] private CollisionShape2D downCollider;
	[Export] private CollisionShape2D leftCollider;
	[Export] private CollisionShape2D rightCollider;

	float upPullback = 0;
	float downPullback = 0;
	float leftPullback = 0;
	float rightPullback = 0;

	private float iTimer = 0;

	public bool IsReadingInputs { get; set; }

	public override void _Ready() {
		base._Ready();

		iTimer = 0.5f;
		GetTree().CreateTimer(0.5f).Timeout += () => {
			IsReadingInputs = true;
		};

		this.BodyEntered += this.OnCollision;

	}

	private void OnCollision(Node body) {
		if (body is CollisionObject2D pBody) {
			if (pBody.GetCollisionLayerValue(1)) {
				springBounce.Play();
			}
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (iTimer > 0) {
			iTimer -= (float) delta;

			if (iTimer <= 0) {
				this.Modulate = Colors.White;
			}
		}

	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (IsReadingInputs) {
			if (Input.IsActionPressed("ui_down")) {
				downPullback = Mathf.MoveToward(downPullback, 1, (float) delta);
				if (downCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100 * (1 - downPullback));
				}
			} else if (Input.IsActionJustReleased("ui_down")) {
				if (downCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100);
				}
				CheckSpring(downRay, downPullback);
				downPullback = 0;
			}

			if (Input.IsActionPressed("ui_up")) {
				upPullback = Mathf.MoveToward(upPullback, 1, (float) delta);
				if (upCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100 * (1 - upPullback));
				}
			} else if (Input.IsActionJustReleased("ui_up")) {
				if (upCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100);
				}
				CheckSpring(upRay, upPullback);
				upPullback = 0;
			}

			if (Input.IsActionPressed("ui_left")) {
				leftPullback = Mathf.MoveToward(leftPullback, 1, (float) delta);
				if (leftCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100 * (1 - leftPullback), 32);
				}
			} else if (Input.IsActionJustReleased("ui_left")) {
				if (leftCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100, 32);
				}
				CheckSpring(leftRay, leftPullback);
				leftPullback = 0;
			}

			if (Input.IsActionPressed("ui_right")) {
				rightPullback = Mathf.MoveToward(rightPullback, 1, (float) delta);
				if (rightCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100 * (1 - rightPullback), 32);
				}
			} else if (Input.IsActionJustReleased("ui_right")) {
				if (rightCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100, 32);
				}
				CheckSpring(rightRay, rightPullback);
				rightPullback = 0;
			}
		}

		QueueRedraw();

	}

	private void CheckSpring(RayCast2D ray, float power) {
		if (ray.GetCollider() is Node collider) {
			if (collider is RigidBody2D body) {
				body.ApplyImpulse(ray.TargetPosition.Normalized() * springCollisionForce * power, ray.GetCollisionPoint());
			}

			if (collider is EnemyBase enemy) {
				enemy.Velocity += ray.TargetPosition.Normalized() * springCollisionForce * power;
			}

			ApplyCentralImpulse(-ray.TargetPosition.Normalized() * springCollisionForce * power);

			springLaunch.Play();
		} else {
			ApplyCentralImpulse(-ray.TargetPosition.Normalized() * springEmptyForce * power);

			springLaunch.Play();
		}
	}

	public override void _Draw() {
		base._Draw();

		DrawRect(new Rect2(downRay.Position - new Vector2(16, 0), new Vector2(32, 50 * (1 - downPullback))), Colors.Gray);
		DrawRect(new Rect2(upRay.Position - new Vector2(16, 50 * (1 - upPullback)), new Vector2(32, 50 * (1 - upPullback))), Colors.Gray);
		DrawRect(new Rect2(leftRay.Position - new Vector2(50 * (1 - leftPullback), 16), new Vector2(50 * (1 - leftPullback), 32)), Colors.Gray);
		DrawRect(new Rect2(rightRay.Position - new Vector2(0, 16), new Vector2(50 * (1 - rightPullback), 32)), Colors.Gray);

	}

	public void AttemptHurt() {
		if (iTimer > 0) return;
		Basket.EjectContents();

		this.Modulate = new Color(1, 1, 1, 0.5f);
		iTimer = invincibilityTimer;
	}

	public int GetRemainingHealth() {
		return Basket.GetChild(0).GetChildCount();
	}

}
