using Godot;
using System;
using DataVault;
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
		public float SpringPullbackSpeed { get; set; } = 1f;
		public float CameraZoom { get; set; } = 0.3f;
	}

	public class GameStats {
		public uint StarCount { get; set; } = 0;
	}

	private Dictionary<string, LevelData> levelData;
	private string lastLevelName;
	private uint lastStarGain;
	public bool IsInLevel { get; set; } = false;

	private List<string> unlockedContent;

	public GameSettings Settings { get; set; }
	public GameStats Stats { get; set; }

	private GameManager() {
		levelData = new Dictionary<string, LevelData>();
		Settings = new GameSettings();
		Stats = new GameStats();
		unlockedContent = new List<string>() {
			"Apple",
			"Wine",
			"Pie"
		};

		LoadSettings();
		LoadGame();
	}

	public void StartLevel(string thisLevelPath, string nextLevelPath) {
		UnlockLevel(thisLevelPath);
		levelData[thisLevelPath].ScenePath = thisLevelPath;
		levelData[thisLevelPath].NextLevelPath = nextLevelPath;

		lastStarGain = 0;

		lastLevelName = thisLevelPath;

		IsInLevel = true;
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

		int newScore = player.GetRemainingHealth();

		if (newScore > level.Score) {
			int difference = newScore - level.Score;
			level.Score = newScore;

			lastStarGain = (uint) difference;
			Stats.StarCount += lastStarGain;
		}
		
		level.WasWon = true;

		if (!string.IsNullOrEmpty(level.NextLevelPath)) UnlockLevel(level.NextLevelPath);

		lastLevelName = thisLevelPath;

		IsInLevel = false;

		SaveGame();
	}

	public LevelData GetLastLevelData() {
		return levelData[lastLevelName];
	}

	public uint GetLastLevelStarGain() {
		return lastStarGain;
	}

	public int GetUnlockedLevelCount() {
		return levelData.Count;
	}

	public LevelData[] GetAllLevelData() {

		LevelData[] levels = new LevelData[levelData.Count];
		levelData.Values.CopyTo(levels, 0);
		return levels;

	}

	public void SaveSettings() {
		Vault settingsVault = VaultManager.GetVault("settings");

		settingsVault ??= VaultManager.CreateVault("settings");

		// Controls
		settingsVault.SetValue("invert_horizontal", Settings.InvertLeftRight);
		settingsVault.SetValue("invert_vertical", Settings.InvertUpDown);
		settingsVault.SetValue("spring_pullback_speed", Settings.SpringPullbackSpeed);

		// Visuals
		settingsVault.SetValue("camera_zoom", Settings.CameraZoom);

		// Volume
		settingsVault.SetValue("volume_master", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master")));
		settingsVault.SetValue("volume_music", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music")));
		settingsVault.SetValue("volume_sfx", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("SFX")));
		settingsVault.SetValue("volume_sfx_ui", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("UI SFX")));
		settingsVault.SetValue("volume_sfx_player", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Player SFX")));
		settingsVault.SetValue("volume_sfx_enemy", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Enemy SFX")));

		settingsVault.SetValue("mute_master", AudioServer.IsBusMute(AudioServer.GetBusIndex("Master")));
		settingsVault.SetValue("mute_music", AudioServer.IsBusMute(AudioServer.GetBusIndex("Music")));
		settingsVault.SetValue("mute_sfx", AudioServer.IsBusMute(AudioServer.GetBusIndex("SFX")));
		settingsVault.SetValue("mute_sfx_ui", AudioServer.IsBusMute(AudioServer.GetBusIndex("UI SFX")));
		settingsVault.SetValue("mute_sfx_player", AudioServer.IsBusMute(AudioServer.GetBusIndex("Player SFX")));
		settingsVault.SetValue("mute_sfx_enemy", AudioServer.IsBusMute(AudioServer.GetBusIndex("Enemy SFX")));

		VaultManager.SaveVault("settings");
	}

	public void LoadSettings() {
		VaultManager.LoadVault("settings");
		Vault settingsVault = VaultManager.GetVault("settings");

		if (settingsVault != null) {
			Settings.InvertLeftRight = settingsVault.GetValue("invert_horizontal").AsBool();
			Settings.InvertUpDown = settingsVault.GetValue("invert_vertical").AsBool();

			Settings.SpringPullbackSpeed = settingsVault.GetValue("spring_pullback_speed").As<float>();
			Settings.CameraZoom = settingsVault.GetValue("camera_zoom").As<float>();

			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), settingsVault.GetValue("volume_master").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Music"), settingsVault.GetValue("volume_music").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("SFX"), settingsVault.GetValue("volume_sfx").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("UI SFX"), settingsVault.GetValue("volume_sfx_ui").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Player SFX"), settingsVault.GetValue("volume_sfx_player").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Enemy SFX"), settingsVault.GetValue("volume_sfx_enemy").As<float>());

			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), settingsVault.GetValue("mute_master").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), settingsVault.GetValue("mute_music").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), settingsVault.GetValue("mute_sfx").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("UI SFX"), settingsVault.GetValue("mute_sfx_ui").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Player SFX"), settingsVault.GetValue("mute_sfx_player").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Enemy SFX"), settingsVault.GetValue("mute_sfx_enemy").AsBool());
		}
	}

	public void SaveGame() {
		Vault gameSaveVault = VaultManager.GetVault("save");

		gameSaveVault ??= VaultManager.CreateVault("save");

		// Level Save
		Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>> levelDataArray = new Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>>();

		foreach (LevelData level in this.levelData.Values) {
			levelDataArray.Add(level.ConvertToVariant());
		}

		gameSaveVault.SetValue("levels", levelDataArray);

		// Stats
		Godot.Collections.Dictionary<string, Variant> statsObj = new Godot.Collections.Dictionary<string, Variant> {
			{ "star_count", Stats.StarCount }
		};

		gameSaveVault.SetValue("stats", statsObj);

		// Content
		Godot.Collections.Array<string> unlockedContentArray = new Godot.Collections.Array<string>();
		foreach (string content in this.unlockedContent) {
			unlockedContentArray.Add(content);
		}

		gameSaveVault.SetValue("unlocked_content", unlockedContentArray);

		VaultManager.SaveVault("save");
	}

	public void LoadGame() {
		VaultManager.LoadVault("save");
		Vault gameSaveVault = VaultManager.GetVault("save");

		if (gameSaveVault != null) {

			Godot.Collections.Dictionary<string, Variant> statsObj = gameSaveVault.GetValue("stats").AsGodotDictionary<string, Variant>();
			if (statsObj.TryGetValue("star_count", out Variant starCountValue)) {
				Stats.StarCount = starCountValue.As<uint>();
			}

			levelData.Clear();
			Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>> levelDataArray = gameSaveVault.GetValue("levels").AsGodotArray<Godot.Collections.Dictionary<string, Variant>>();

			foreach (Godot.Collections.Dictionary<string, Variant> level in levelDataArray) {
				LevelData newLevelData = LevelData.LoadFromVariant(level);
				levelData.Add(newLevelData.ScenePath, newLevelData);
			}

			Godot.Collections.Array<string> unlockedContentArray = gameSaveVault.GetValue("unlocked_content").AsGodotArray<string>();
			if (unlockedContentArray != null) {
				this.unlockedContent.Clear();
				foreach (string content in unlockedContentArray) {
					this.unlockedContent.Add(content);
				}
			}
		}
	}

	public void DeleteSavedGame() {
		levelData.Clear();
		Stats = new GameStats();
		unlockedContent = new List<string>() {
			"Apple",
			"Wine",
			"Pie"
		};
		SaveGame();
	}

	public void DeleteSavedSettings() {
		Vault settingsVault = VaultManager.GetVault("settings");

		settingsVault ??= VaultManager.CreateVault("settings");

		// Controls
		settingsVault.SetValue("invert_horizontal", false);
		settingsVault.SetValue("invert_vertical", false);

		// Volume
		settingsVault.SetValue("volume_master", 1);
		settingsVault.SetValue("volume_music", 1);
		settingsVault.SetValue("volume_sfx", 1);
		settingsVault.SetValue("mute_master", false);
		settingsVault.SetValue("mute_music", false);
		settingsVault.SetValue("mute_sfx", false);

		VaultManager.SaveVault("settings");

		LoadSettings();
	}

	public void UnlockContent(string content) {
		unlockedContent.Add(content);

		SaveGame();
	}

	public bool IsUnlocked(string content) {
		return unlockedContent.Contains(content);
	}

	public List<string> GetAllUnlocked() {
		return unlockedContent;
	}

}
