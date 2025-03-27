using Godot;
using System;

public partial class SetGameSettings : Node {

	[Export] private VolumeSlider[] volumeSliders;
	[Export] private MuteBusButton[] muteButtons;
	[Export] private BetterOptionButton invertControls;

	public override void _Ready() {
		base._Ready();

		SetInvertControlsButtonState();
	}

	private void SetInvertControlsButtonState() {
		GameManager.GameSettings settings = GameManager.Instance.Settings;
		if (settings.InvertUpDown == settings.InvertLeftRight) {
			if (settings.InvertUpDown) {
				invertControls.Select(3);
			} else {
				invertControls.Select(0);
			}
		} else {
			if (settings.InvertUpDown) {
				invertControls.Select(2);
			} else {
				invertControls.Select(1);
			}
		}
	}

	public void ChangeControlsInvert(int index) {
		GameManager.GameSettings settings = GameManager.Instance.Settings;
		if (index == 0) {
			settings.InvertUpDown = false;
			settings.InvertLeftRight = false;
		} else if (index == 1) {
			settings.InvertUpDown = false;
			settings.InvertLeftRight = true;
		} else if (index == 2) {
			settings.InvertUpDown = true;
			settings.InvertLeftRight = false;
		} else if (index == 3) {
			settings.InvertUpDown = true;
			settings.InvertLeftRight = true;
		}

		GameManager.Instance.SaveSettings();
	}

	public void ResetSettings() {
		GameManager.Instance.DeleteSavedSettings();

		SetInvertControlsButtonState();
		foreach (VolumeSlider slider in volumeSliders) {
			slider.Reset();
		}
		foreach (MuteBusButton btn in muteButtons) {
			btn.Reset();
		}
	}

	public void ResetGame() {
		GameManager.Instance.DeleteSavedGame();
	}

}
