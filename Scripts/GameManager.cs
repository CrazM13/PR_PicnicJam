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

	public class GameSettings {
		public bool InvertUpDown { get; set; } = false;
		public bool InvertLeftRight { get; set; } = false;
	}

	private Dictionary<string, LevelData> levelData;
	private string lastLevelName;

	public GameSettings Settings { get; set; }

	private GameManager() {
		levelData = new Dictionary<string, LevelData>();
		Settings = new GameSettings();
	}

	public void StartLevel(string thisLevelPath, string nextLevelPath) {
		if (string.IsNullOrEmpty(nextLevelPath)) {
			nextLevelPath = thisLevelPath;
		}

		UnlockLevel(thisLevelPath);
		levelData[thisLevelPath].ScenePath = thisLevelPath;
		levelData[thisLevelPath].NextLevelPath = nextLevelPath;

		lastLevelName = thisLevelPath;
	}

	public void UnlockLevel(string levelPath) {
		if (!levelData.ContainsKey(levelPath)) {
			levelData.Add(levelPath, new LevelData() {
				ScenePath = levelPath,
				NextLevelPath = levelPath,
				Score = 0,
				WasWon = false
			});
		}
	}

	public void CompleteLevel(string thisLevelPath, SimplePlayerController player) {
		LevelData level = levelData[thisLevelPath];
		level.Score = player.GetRemainingHealth();
		level.WasWon = true;

		UnlockLevel(level.NextLevelPath);

		lastLevelName = thisLevelPath;
	}

	public LevelData GetLastLevelData() {
		return levelData[lastLevelName];
	}

	public int GetUnlockedLevelCount() {
		return levelData.Count;
	}

	public LevelData[] GetAllLevelData() {

		LevelData[] levels = new LevelData[levelData.Count];
		levelData.Values.CopyTo(levels, 0);
		return levels;

	}

}
