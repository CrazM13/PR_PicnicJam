using Godot;
using System;

public partial class BetterTabMenu : TabContainer {

	[ExportGroup("Audio")]
	[Export] private AudioStreamPlayer clickSFX;
	[Export] private AudioStreamPlayer hoverSFX;

	public override void _Ready() {
		base._Ready();

		this.TabChanged += (long _) => {
			clickSFX.Play();
		};

		this.TabHovered += (long _) => {
			hoverSFX.Play();
		};

	}
}
