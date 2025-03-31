using Godot;
using System;

public partial class DisplayStarCount : Label {

	[Export] private bool updateLive = false;

	public override void _Ready() {
		base._Ready();

		this.Text = $"x{GameManager.Instance.Stats.StarCount:D3}";

	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (updateLive) {
			this.Text = $"x{GameManager.Instance.Stats.StarCount:D3}";
		}

	}

}
