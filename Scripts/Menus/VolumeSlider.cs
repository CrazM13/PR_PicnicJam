using Godot;
using System;

public partial class VolumeSlider : BetterHSlider {

	[Export(PropertyHint.EnumSuggestion, "Master,Music,SFX,UI SFX,Player SFX,Enemy SFX")] private StringName bus = "Master";

	public override void _Ready() {
		base._Ready();

		Reset();
		this.ValueChanged += this.OnValueChanged;

	}

	public void Reset() {
		this.Value = AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex(bus));
	}

	private void OnValueChanged(double value) {
		AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex(bus), (float) value);

		GameManager.Instance.SaveSettings();
	}
}
