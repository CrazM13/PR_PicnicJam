using Godot;
using System;

public partial class BetterHSlider : HSlider {

	[Export] private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();

		this.ValueChanged += this.OnInternalValueChanged;

		this.MouseEntered += () => {
			audio.Play();
		};

	}

	private void OnInternalValueChanged(double value) {
		audio.Play();
	}

}
