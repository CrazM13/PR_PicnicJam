using Godot;
using System;

public partial class GoalArea : Area2D {

	[Export] private AudioStream levelMusic;
	[Export] private AudioStream scoreMusic;
	[Export] private string nextScene;

	[Export] private AnimationPlayer[] puppets;

	private string levelName;

	public override void _Ready() {
		base._Ready();

		this.BodyEntered += this.OnBodyEntered;

		levelName = GetTree().CurrentScene.SceneFilePath;
		GameManager.Instance.StartLevel(levelName, nextScene);
		BackgroundMusic.Instance.ChangeTrack(levelMusic);
	}

	private void OnBodyEntered(Node2D body) {
		if (body is SimplePlayerController player) {
			player.IsReadingInputs = false;
			GameManager.Instance.CompleteLevel(levelName, player);
			Engine.TimeScale = 0.05f;
			BackgroundMusic.Instance.ChangeTrack(scoreMusic);

			foreach(AnimationPlayer puppet in puppets) {
				puppet.SpeedScale = 1 / (float)Engine.TimeScale;
				puppet.Play("cheer");
			}

			GetTree().CreateTimer(3 * Engine.TimeScale).Timeout += () => {
				Engine.TimeScale = 1f;
				SceneManager.Instance.LoadScene("res://Scenes/ScoreScene.tscn");
			};
			
		}
	}
}
