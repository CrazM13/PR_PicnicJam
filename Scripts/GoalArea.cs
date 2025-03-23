using Godot;
using System;

public partial class GoalArea : Area2D {

	[Export] private string nextScene;

	private string levelName;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEntered;

		levelName = GetTree().CurrentScene.SceneFilePath;
		GameManager.Instance.StartLevel(levelName, nextScene);
	}

	private void OnBodyEntered(Node2D body) {
		if (body is SimplePlayerController player) {
			GameManager.Instance.CompleteLevel(levelName, player);

			SceneManager.Instance.LoadScene("res://Scenes/ScoreScene.tscn");
		}
	}
}
