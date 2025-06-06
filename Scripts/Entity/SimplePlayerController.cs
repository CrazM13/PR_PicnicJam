using Godot;
using System;

public partial class SimplePlayerController : RigidBody2D {

	[Export] private float springCollisionForce = 1000f;
	[Export] private float springEmptyForce = 1000f;
	[Export] private float invincibilityTimer = 2.5f;

	[ExportGroup("References")]
	[Export] public HeldObjectArea Basket { get; private set; }

	[ExportSubgroup("Sprites")]
	[Export] private Sprite2D springUp;
	[Export] private Sprite2D springDown;
	[Export] private Sprite2D springLeft;
	[Export] private Sprite2D springRight;

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

		float springPullbackSpeed = GameManager.Instance.Settings.SpringPullbackSpeed;

		if (IsReadingInputs) {
			if (Input.IsActionPressed(GetDownInput())) {
				downPullback = Mathf.MoveToward(downPullback, 1, (float) delta * springPullbackSpeed);
				if (downCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100 * (1 - downPullback));
				}
			} else if (Input.IsActionJustReleased(GetDownInput())) {
				if (downCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100);
				}
				CheckSpring(downRay, downPullback);
				downPullback = 0;
			}

			if (Input.IsActionPressed(GetUpInput())) {
				upPullback = Mathf.MoveToward(upPullback, 1, (float) delta * springPullbackSpeed);
				if (upCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100 * (1 - upPullback));
				}
			} else if (Input.IsActionJustReleased(GetUpInput())) {
				if (upCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(32, 100);
				}
				CheckSpring(upRay, upPullback);
				upPullback = 0;
			}

			if (Input.IsActionPressed(GetLeftInput())) {
				leftPullback = Mathf.MoveToward(leftPullback, 1, (float) delta * springPullbackSpeed);
				if (leftCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100 * (1 - leftPullback), 32);
				}
			} else if (Input.IsActionJustReleased(GetLeftInput())) {
				if (leftCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100, 32);
				}
				CheckSpring(leftRay, leftPullback);
				leftPullback = 0;
			}

			if (Input.IsActionPressed(GetRightInput())) {
				rightPullback = Mathf.MoveToward(rightPullback, 1, (float) delta * springPullbackSpeed);
				if (rightCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100 * (1 - rightPullback), 32);
				}
			} else if (Input.IsActionJustReleased(GetRightInput())) {
				if (rightCollider.Shape is RectangleShape2D shape) {
					shape.Size = new Vector2(100, 32);
				}
				CheckSpring(rightRay, rightPullback);
				rightPullback = 0;
			}
		}

		UpdateSprites();

	}

	private void UpdateSprites() {
		springUp.Scale = new Vector2(1 - upPullback, 1);
		springDown.Scale = new Vector2(1 - downPullback, 1);

		springLeft.Scale = new Vector2(1 - leftPullback, 1);
		springRight.Scale = new Vector2(1 - rightPullback, 1);
	}

	private void CheckSpring(RayCast2D ray, float power) {
		if (ray.GetCollider() is Node collider) {
			if (collider is RigidBody2D body) {
				body.ApplyImpulse(ray.TargetPosition.Normalized() * springCollisionForce * power * 10, ray.GetCollisionPoint());
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

	public void AttemptHurt() {
		if (iTimer > 0) return;
		Basket.EjectContents();

		this.Modulate = new Color(1, 1, 1, 0.5f);
		iTimer = invincibilityTimer;
	}

	public int GetRemainingHealth() {
		return Basket.GetChild(0).GetChildCount();
	}

	private string GetUpInput() {
		return GameManager.Instance.Settings.InvertUpDown ? "move_down" : "move_up";
	}

	private string GetDownInput() {
		return GameManager.Instance.Settings.InvertUpDown ? "move_up" : "move_down";
	}

	private string GetLeftInput() {
		return GameManager.Instance.Settings.InvertLeftRight ? "move_right" : "move_left";
	}

	private string GetRightInput() {
		return GameManager.Instance.Settings.InvertLeftRight ? "move_left" : "move_right";
	}

}
