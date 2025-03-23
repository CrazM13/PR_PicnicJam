using Godot;
using System;

public partial class HeldObjectArea : Area2D {

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
	}

	private void OnBodyEnter(Node2D body) {
		if (body is HeldObject heldObj) {
			if (heldObj.IsCollectable) CallDeferred("AddToChildren", heldObj);
		}
	}

	private void AddToChildren(HeldObject heldObj) {
		if (heldObj.GetParent() != this) {
			Vector2 gPos = heldObj.GlobalPosition;
			heldObj.GetParent().RemoveChild(heldObj);
			GetChild(0).AddChild(heldObj);
			heldObj.GlobalPosition = gPos;

			heldObj.IsCollectable = false;
			heldObj.IsRunningPhysics = false;
		}
	}

	private void RemoveFromChildren(HeldObject heldObj) {
		if (heldObj.GetParent() == GetChild(0)) {
			Vector2 gPos = heldObj.GlobalPosition;
			GetChild(0).RemoveChild(heldObj);
			GetTree().CurrentScene.AddChild(heldObj);
			heldObj.GlobalPosition = gPos;

			heldObj.IsRunningPhysics = true;
		}
	}

	public void EjectContents() {
		Node2D container = GetChild<Node2D>(0);

		if (container.GetChildCount() > 1) {
			if (container.GetChild(0) is HeldObject heldObj) {
				CallDeferred("RemoveFromChildren", heldObj);
				heldObj.Velocity += Vector2.Up * 1000;
			}
		} else {
			SceneManager.Instance.ReloadScene();
		}

	}
}
