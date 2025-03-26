using Godot;
using System;

public partial class MuteBusButton : BetterButton {

	[Export(PropertyHint.EnumSuggestion, "Master,Music,SFX")] private StringName bus = "Master";
	[Export] private VolumeSlider slider;
	[Export] private TextureRect icon;

	[Export] private Texture2D muteIcon;
	[Export] private Texture2D unmuteIcon;

	public override void _Ready() {
		base._Ready();

		if (AudioServer.IsBusMute(AudioServer.GetBusIndex(bus))) {
			this.ButtonPressed = true;
			icon.Texture = muteIcon;
			slider.Editable = false;
		}

		this.Toggled += this.ToggleMute;
	}

	private void ToggleMute(bool toggledOn) {
		AudioServer.SetBusMute(AudioServer.GetBusIndex(bus), toggledOn);
		slider.Editable = !toggledOn;

		icon.Texture = toggledOn ? muteIcon : unmuteIcon;

		GameManager.Instance.SaveSettings();
	}
}
