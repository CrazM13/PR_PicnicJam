using Godot;
using System;

public partial class LevelSelectScroll : ScrollContainer {

	[Export] private float speed = 100;

	private float targetScroll = 0;
	private int index = 0;

	private string[] levels;

	public override void _Ready() {
		base._Ready();

		LevelData[] levelData = GameManager.Instance.GetAllLevelData();
		levels = new string[levelData.Length];
		for (int i = 0; i < levels.Length; i++) {
			levels[i] = levelData[i].ScenePath;
		}
	}

	public void ScrollNext() {
		float maxVal = (float) this.GetHScrollBar().MaxValue;
		if (targetScroll + 300 < maxVal) {
			targetScroll += 260;
			index++;
		}
	}

	public void ScrollLast() {
		if (targetScroll > 0) {
			targetScroll -= 260;
			index--;
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);

		this.ScrollHorizontal = (int)Mathf.MoveToward(this.ScrollHorizontal, targetScroll, (float) delta * speed);
	}

	public void LoadLevel() {
		SceneManager.Instance.LoadScene(levels[index]);
	}

}
