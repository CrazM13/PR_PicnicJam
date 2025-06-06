using Godot;
using System;

public partial class BetterOptionButton : OptionButton {

	[ExportGroup("Audio")]
	[Export] private AudioStreamPlayer clickSFX;
	[Export] private AudioStreamPlayer hoverSFX;
	[Export] private AudioStreamPlayer clickDisabledSFX;
	[Export] private AudioStreamPlayer hoverDisabledSFX;

	public override void _Ready() {
		base._Ready();

		this.ButtonDown += () => {
			if (!Disabled) clickSFX.Play();
			else clickDisabledSFX.Play();
		};
		this.ItemSelected += (long _) => {
			if (!Disabled) clickSFX.Play();
			else clickDisabledSFX.Play();
		};
		this.MouseEntered += () => {
			if (!Disabled) hoverSFX.Play();
			else hoverDisabledSFX.Play();
		};
	}

}
