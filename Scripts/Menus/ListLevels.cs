using Godot;
using System;

public partial class ListLevels : Control {

	[Export] private PackedScene levelPrefab;

	public override void _Ready() {
		base._Ready();

		CallDeferred("AdjustSize");

		foreach (LevelData level in GameManager.Instance.GetAllLevelData()) {
			Control levelNode = levelPrefab.Instantiate<Control>();

			AddChild(levelNode);
		}

	}

	private void AdjustSize() {
		this.Size = new Vector2(256 * GameManager.Instance.GetUnlockedLevelCount(), 256);
	}

}
