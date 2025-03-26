using Godot;
using System;

namespace DataVault {



	public static class VaultEncryption {

		public static string BasicEncrypt(string data, string key) {
			string encryptedData = XORString(data, key);
			encryptedData = CypherString(encryptedData, key);
			return encryptedData;
		}

		public static string BasicDecrypt(string encryptedData, string key) {
			string decryptedData = CypherString(encryptedData, key, true);
			decryptedData = XORString(decryptedData, key);
			return decryptedData;
		}

		private static string XORString(string data, string key) {
			string xorData = "";

			for (int i = 0; i < data.Length; i++) {
				xorData += (char)(data[i] ^ key[i % key.Length]);
			}

			return xorData;
		}

		private static string CypherString(string data, string key, bool decrypt = false) {
			string cypherData = "";

			for (int i = 0; i < data.Length; i++) {
				unchecked {
					cypherData += (char)(data[i] + (key[i % key.Length] * (decrypt ? -1 : 1)));
				}
			}

			return cypherData;
		}

	}
}
