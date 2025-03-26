using Godot;
using System;
using Godot.Collections;

public class LevelData {

	public string ScenePath { get; set; }
	public string NextLevelPath { get; set; }
	public int Score { get; set; }
	public bool WasWon { get; set; }

	public Dictionary<string, Variant> ConvertToVariant() {
		Dictionary<string, Variant> data = new Dictionary<string, Variant>();

		data.Add("ScenePath", ScenePath);
		data.Add("NextLevelPath", NextLevelPath);
		data.Add("Score", Score);
		data.Add("WasWon", WasWon);

		return data;
	}

	public static LevelData LoadFromVariant(Dictionary<string, Variant> data) {
		return new LevelData() {
			ScenePath = data["ScenePath"].AsString(),
			NextLevelPath = data["NextLevelPath"].AsString(),
			Score = data["Score"].As<int>(),
			WasWon = data["WasWon"].AsBool()
		};
	}

}
