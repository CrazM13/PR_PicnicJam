using Godot;
using System;

public partial class ShopButton : BetterTextButton {

	[Export] private Shop shop;
	[Export] private string content;
	[Export] private uint cost;

	public override void _Ready() {
		base._Ready();

		if (GameManager.Instance.IsUnlocked(content)) {
			this.Text = "Unlocked";
			this.Disabled = true;
		}

		this.Pressed += this.OnPressed;

	}

	private void OnPressed() {
		shop.AttemptBuy(content, cost);

		this.Text = "Unlocked";
		this.Disabled = true;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (!this.Disabled) {
			bool newDisabled = GameManager.Instance.Stats.StarCount < cost;

			if (newDisabled) {
				this.Text = "Too Expensive";
				this.Disabled = true;
			}
		}

	}

}
