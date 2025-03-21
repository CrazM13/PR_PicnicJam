using Godot;
using System;

public partial class HeldObjectArea : Area2D {

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEnter;
		//this.BodyExited += this.OnBodyExit;

	}

	private void OnBodyExit(Node2D body) {
		if (body is HeldObject heldObj) {
			CallDeferred("RemoveFromChildren", heldObj);
		}
	}

	private void OnBodyEnter(Node2D body) {
		if (body is HeldObject heldObj) {
			CallDeferred("AddToChildren", heldObj);
		}
	}

	private void AddToChildren(HeldObject heldObj) {
		if (heldObj.GetParent() != this) {
			Vector2 gPos = heldObj.GlobalPosition;
			heldObj.GetParent().RemoveChild(heldObj);
			AddChild(heldObj);
			heldObj.GlobalPosition = gPos;
		}
	}

	private void RemoveFromChildren(HeldObject heldObj) {
		if (heldObj.GetParent() == this) {
			Vector2 gPos = heldObj.GlobalPosition;
			this.RemoveChild(heldObj);
			GetTree().CurrentScene.AddChild(heldObj);
			heldObj.GlobalPosition = gPos;
		}
	}
}
