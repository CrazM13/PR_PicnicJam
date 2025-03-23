using Godot;
using System;

public partial class GoalArea : Area2D {

	[Export] private string nextScene;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEntered;

	}

	private void OnBodyEntered(Node2D body) {
		if (body is SimplePlayerController player) {
			if (string.IsNullOrEmpty(nextScene)) {
				SceneManager.Instance.ReloadScene();
			} else {
				SceneManager.Instance.LoadScene(nextScene);
			}
		}
	}
}
