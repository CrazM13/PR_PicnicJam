using Godot;
using System;

public partial class PercentChanger : Control {

	[Signal] public delegate void PercentChangedEventHandler(float percent);

	private float percentage;

	
	[Export] private float maxPercent = 1f;
	[Export] private float minPercent = 0f;
	[Export] private float step = 0.5f;
	[Export(PropertyHint.Range, "0,2")] private int decimalPoints = 0;
	[ExportGroup("References")]
	[Export] private Label label;

	public void RaisePercent() {
		if (percentage + step < maxPercent) {
			percentage += step;
			label.Text = percentage.ToString($"P{decimalPoints}");
			EmitSignal(SignalName.PercentChanged, percentage);
		}
	}

	public void LowerPercent() {
		if (percentage > minPercent) {
			percentage -= step;
			label.Text = percentage.ToString($"P{decimalPoints}");
			EmitSignal(SignalName.PercentChanged, percentage);
		}
	}

	public void SetPercent(float percentage, bool emitSignal = true) {
		this.percentage = percentage;
		label.Text = percentage.ToString($"P{decimalPoints}");
		if (emitSignal) EmitSignal(SignalName.PercentChanged, this.percentage);
	}

	public float GetPercent() {
		return percentage;
	}

}
