using Godot;
using System;
using System.Collections.Generic;

namespace DataVault {
	public static class VaultManager {

		private static Dictionary<string, Vault> vaults = new Dictionary<string, Vault>();

		public static VaultMode VaultEncodingMode { get; set; } = VaultMode.JSON;

		public static void LoadVault(string vaultName) {

			string jsonString = "";

			if (FileAccess.FileExists($"user://vaults/{vaultName}.bin")) {
				using FileAccess file = FileAccess.OpenCompressed($"user://vaults/{vaultName}.bin", FileAccess.ModeFlags.Read);
				jsonString = VaultEncryption.BasicDecrypt(file.GetVar().AsString(), vaultName);
			} else if (FileAccess.FileExists($"user://vaults/{vaultName}.sav")) {
				using FileAccess file = FileAccess.OpenCompressed($"user://vaults/{vaultName}.sav", FileAccess.ModeFlags.Read);
				jsonString = VaultEncryption.BasicDecrypt(file.GetAsText(), vaultName);
			} else if (FileAccess.FileExists($"user://vaults/{vaultName}.json")) {
				using FileAccess file = FileAccess.Open($"user://vaults/{vaultName}.json", FileAccess.ModeFlags.Read);
				jsonString = file.GetAsText();
			} else {
				GD.PrintErr("[!!!] No vault found!");
			}

			if (!string.IsNullOrEmpty(jsonString)) {
				Json jsonData = new Json();
				jsonData.Parse(jsonString);

				Vault newVault = new Vault(jsonData);

				if (vaults.ContainsKey(vaultName)) {
					vaults[vaultName] = newVault;
				} else {
					vaults.Add(vaultName, newVault);
				}
			}
		}

		public static void UnloadVault(string vaultName) {
			if (vaults.ContainsKey(vaultName)) {
				vaults.Remove(vaultName);
			}
		}

		public static Vault GetVault(string vaultName) {
			if (vaults.ContainsKey(vaultName)) {
				return vaults[vaultName];
			}

			return null;
		}

		public static void SaveVault(string vaultName) {
			if (vaults.ContainsKey(vaultName)) {
				if (!DirAccess.DirExistsAbsolute("user://vaults/")) {
					DirAccess.MakeDirAbsolute("user://vaults/");
				}

				string[] dirs = vaultName.Split('/');
				string fullDir = "user://vaults/";
				for (int i = 0; i < dirs.Length - 1; i++) {
					fullDir += dirs[i];
					if (!DirAccess.DirExistsAbsolute(fullDir)) {
						DirAccess.MakeDirAbsolute(fullDir);
					}
				}
				
				if (VaultEncodingMode == VaultMode.BINARY) {
					using FileAccess file = FileAccess.OpenCompressed($"user://vaults/{vaultName}.bin", FileAccess.ModeFlags.Write);
					file.StoreVar(VaultEncryption.BasicEncrypt(vaults[vaultName].ToString(), vaultName));
				} else if (VaultEncodingMode == VaultMode.SAV) {
					using FileAccess file = FileAccess.OpenCompressed($"user://vaults/{vaultName}.sav", FileAccess.ModeFlags.Write);
					file.StoreString(VaultEncryption.BasicEncrypt(vaults[vaultName].ToString(), vaultName));
				} else {
					using FileAccess file = FileAccess.Open($"user://vaults/{vaultName}.json", FileAccess.ModeFlags.Write);
					file.StoreString(vaults[vaultName].ToString());
				}
				
			}
		}

		public static Vault CreateVault(string vaultName) {
			Vault newVault = new Vault(vaultName);

			if (vaults.ContainsKey(vaultName)) {
				vaults[vaultName] = newVault;
			} else {
				vaults.Add(vaultName, newVault);
			}

			return newVault;
		}

	}
}
