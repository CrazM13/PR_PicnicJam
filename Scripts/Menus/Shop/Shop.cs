using Godot;
using System;

public partial class Shop : Node {

	public void ReturnToMenu() {
		SceneManager.Instance.LoadScene("res://Scenes/MainMenu.tscn");
	}

	public void AttemptBuy(string content, uint cost) {
		if (GameManager.Instance.Stats.StarCount >= cost) {
			GameManager.Instance.Stats.StarCount -= cost;
			GameManager.Instance.UnlockContent(content);
		}
	}

}
