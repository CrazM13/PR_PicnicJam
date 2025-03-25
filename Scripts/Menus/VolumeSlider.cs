using Godot;
using System;

public partial class VolumeSlider : HSlider {

	[Export(PropertyHint.EnumSuggestion, "Master,Music,SFX")] private StringName bus = "Master";
	[Export] private AudioStreamPlayer audio;

	public override void _Ready() {
		base._Ready();

		this.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex(bus));
		this.ValueChanged += this.OnValueChanged;

		this.MouseEntered += () => {
			audio.Play();
		};

	}

	private void OnValueChanged(double value) {
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex(bus), (float) value);
		audio.Play();
	}
}
