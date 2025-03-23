using Godot;
using System;
using System.Collections.Generic;

public class GameManager {

	#region Singleton
	private static GameManager instance;
	public static GameManager Instance {
		get {
			instance ??= new GameManager();

			return instance;
		}
	}
	#endregion

	private Dictionary<string, LevelData> levelData;
	private string lastLevelName;

	private GameManager() {
		levelData = new Dictionary<string, LevelData>();
	}

	public void StartLevel(string thisLevelPath, string nextLevelPath) {
		if (string.IsNullOrEmpty(nextLevelPath)) {
			nextLevelPath = thisLevelPath;
		}

		if (!levelData.ContainsKey(thisLevelPath)) {
			levelData.Add(thisLevelPath, new LevelData() {
				ScenePath = thisLevelPath,
				NextLevelPath = nextLevelPath,
				Score = 0,
				WasWon = false
			});
		}

		lastLevelName = thisLevelPath;
	}

	public void CompleteLevel(string thisLevelPath, SimplePlayerController player) {
		LevelData level = levelData[thisLevelPath];
		level.Score = player.GetRemainingHealth();
		level.WasWon = true;

		lastLevelName = thisLevelPath;
	}

	public LevelData GetLastLevelData() {
		return levelData[lastLevelName];
	}

}
