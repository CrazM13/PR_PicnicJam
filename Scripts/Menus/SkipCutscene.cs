using Godot;
using System;

public partial class SkipCutscene : SceneNavButton {

	[Export] private string nextScene;

	public override void _Process(double delta) {
		base._Process(delta);

		if (Input.IsActionJustPressed("action_pause")) {
			this.NavToScene(nextScene);
		}

	}

}
