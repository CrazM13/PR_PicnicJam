using Godot;
using System;

public partial class LevelSelectScroll : ScrollContainer {

	[Export] private float speed = 100;

	private float targetScroll = 0;

	public void ScrollNext() {
		float maxVal = (float) this.GetHScrollBar().MaxValue;
		if (targetScroll + 256 < maxVal) {
			targetScroll += 256;
		}
	}

	public void ScrollLast() {
		if (targetScroll - 256 >= 0) {
			targetScroll -= 256;
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);

		this.ScrollHorizontal = (int)Mathf.MoveToward(this.ScrollHorizontal, targetScroll, (float) delta * speed);

	}

}
