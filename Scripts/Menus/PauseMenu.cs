using Godot;
using System;

public partial class PauseMenu : CanvasLayer {

	[Export] private AudioStreamPlayer openMenuSFX;
	[Export] private AudioStreamPlayer closeMenuSFX;

	public override void _Process(double delta) {
		base._Process(delta);

		if (Input.IsActionJustPressed("action_pause")) {
			GetTree().Paused = !GetTree().Paused;

			bool isPaused = GetTree().Paused;
			if (this.Visible != isPaused) {
				this.Visible = isPaused;
				if (isPaused) openMenuSFX.Play();
				else closeMenuSFX.Play();
			}
		}
	}

	public void Unpause() {
		GetTree().Paused = false;
	}

	private void QuitLevel() {
		GameManager.Instance.IsInLevel = false;

		SceneManager.Instance.LoadScene("res://Scenes/MainMenu.tscn");
	}

}
