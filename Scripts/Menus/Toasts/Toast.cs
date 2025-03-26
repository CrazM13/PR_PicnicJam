using Godot;
using System;

public partial class Toast : Control {

	[Export] private TextureRect icon;
	[Export] private Label text;

	[Export] private Curve offset;

	private float duration;
	private float timer;

	public void SetMessage(float duration, string message, Texture2D icon = null) {
		this.icon.Texture = icon;
		this.text.Text = message;
		this.duration = duration;

		this.timer = 0;

		GetChild<Control>(0).Position = new Vector2(offset.Sample(timer / duration), 0);
	}

	public override void _Process(double delta) {
		base._Process(delta);

		timer += (float) delta;

		GetChild<Control>(0).Position = new Vector2(offset.Sample(timer / duration), 0);

		if (timer >= duration) {
			QueueFree();
		}
	}



}
