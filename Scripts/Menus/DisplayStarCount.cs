using Godot;
using System;

public partial class DisplayStarCount : Label {

	public override void _Ready() {
		base._Ready();

		this.Text = $"x{GameManager.Instance.Stats.StarCount:D3}";

	}

}
