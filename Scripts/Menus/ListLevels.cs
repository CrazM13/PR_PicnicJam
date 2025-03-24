using Godot;
using System;

public partial class ListLevels : Control {

	[Export] private PackedScene levelPrefab;

	public override void _Ready() {
		base._Ready();

		CallDeferred("AdjustSize");

		foreach (LevelData level in GameManager.Instance.GetAllLevelData()) {
			LevelDisplay levelNode = levelPrefab.Instantiate<LevelDisplay>();

			levelNode.SetData(level);

			AddChild(levelNode);
		}

	}

	private void AdjustSize() {
		this.CustomMinimumSize = new Vector2(256 * GameManager.Instance.GetUnlockedLevelCount(), 256);
	}

}
