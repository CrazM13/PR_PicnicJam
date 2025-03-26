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
	}

	private Dictionary<string, LevelData> levelData;
	private string lastLevelName;

	public GameSettings Settings { get; set; }

	private GameManager() {
		levelData = new Dictionary<string, LevelData>();
		Settings = new GameSettings();

		LoadSettings();
		LoadGame();
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

		SaveGame();
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

	public void SaveSettings() {
		Vault settingsVault = VaultManager.GetVault("settings");

		settingsVault ??= VaultManager.CreateVault("settings");

		// Controls
		settingsVault.SetValue("invert_horizontal", Settings.InvertLeftRight);
		settingsVault.SetValue("invert_vertical", Settings.InvertUpDown);

		// Volume
		settingsVault.SetValue("volume_master", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Master")));
		settingsVault.SetValue("volume_music", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("Music")));
		settingsVault.SetValue("volume_sfx", AudioServer.GetBusVolumeLinear(AudioServer.GetBusIndex("SFX")));
		settingsVault.SetValue("mute_master", AudioServer.IsBusMute(AudioServer.GetBusIndex("Master")));
		settingsVault.SetValue("mute_music", AudioServer.IsBusMute(AudioServer.GetBusIndex("Music")));
		settingsVault.SetValue("mute_sfx", AudioServer.IsBusMute(AudioServer.GetBusIndex("SFX")));

		VaultManager.SaveVault("settings");
	}

	public void LoadSettings() {
		VaultManager.LoadVault("settings");
		Vault settingsVault = VaultManager.GetVault("settings");

		if (settingsVault != null) {
			Settings.InvertLeftRight = settingsVault.GetValue("invert_horizontal").AsBool();
			Settings.InvertUpDown = settingsVault.GetValue("invert_vertical").AsBool();

			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Master"), settingsVault.GetValue("volume_master").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("Music"), settingsVault.GetValue("volume_music").As<float>());
			AudioServer.SetBusVolumeLinear(AudioServer.GetBusIndex("SFX"), settingsVault.GetValue("volume_sfx").As<float>());

			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), settingsVault.GetValue("mute_master").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), settingsVault.GetValue("mute_music").AsBool());
			AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), settingsVault.GetValue("mute_sfx").AsBool());
		}
	}

	public void SaveGame() {
		Vault gameSaveVault = VaultManager.GetVault("save");

		gameSaveVault ??= VaultManager.CreateVault("save");

		Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>> levelDataArray = new Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>>();

		foreach (LevelData level in this.levelData.Values) {
			levelDataArray.Add(level.ConvertToVariant());
		}

		gameSaveVault.SetValue("levels", levelDataArray);

		VaultManager.SaveVault("save");
	}

	public void LoadGame() {
		VaultManager.LoadVault("save");
		Vault gameSaveVault = VaultManager.GetVault("save");

		if (gameSaveVault != null) {
			levelData.Clear();
			Godot.Collections.Array<Godot.Collections.Dictionary<string, Variant>> levelDataArray = gameSaveVault.GetValue("levels").AsGodotArray<Godot.Collections.Dictionary<string, Variant>>();

			foreach (Godot.Collections.Dictionary<string, Variant> level in levelDataArray) {
				LevelData newLevelData = LevelData.LoadFromVariant(level);
				levelData.Add(newLevelData.ScenePath, newLevelData);
			}

		}
	}

	public void DeleteSavedGame() {
		levelData.Clear();
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
		SceneManager.Instance.ReloadScene();
	}

}
