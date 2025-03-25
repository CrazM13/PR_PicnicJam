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
			player.IsReadingInputs = false;
			GameManager.Instance.CompleteLevel(levelName, player);
			Engine.TimeScale = 0.05f;

			GetTree().CreateTimer(0.05f).Timeout += () => {
				Engine.TimeScale = 1f;
				SceneManager.Instance.LoadScene("res://Scenes/ScoreScene.tscn");
			};
			
		}
	}
}
