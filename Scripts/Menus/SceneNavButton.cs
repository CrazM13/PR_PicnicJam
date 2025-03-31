using Godot;
using System;

public partial class SceneNavButton : Node {

	public void NavToScene(string path) {
		SceneManager.Instance.LoadScene(path);
	}

	public void ReloadScene() {
		SceneManager.Instance.ReloadScene();
	}

	public void NavToNextLevel() {
		string path = GameManager.Instance.GetLastLevelData().NextLevelPath;

		if (string.IsNullOrEmpty(path)) {
			SceneManager.Instance.LoadScene("res://Scenes/OutroScene.tscn");
		} else {
			SceneManager.Instance.LoadScene(path);
		}
	}

	public void NavToLastLevel() {
		string path = GameManager.Instance.GetLastLevelData().ScenePath;

		SceneManager.Instance.LoadScene(path);
	}

	public void Quit() {
		SceneManager.Instance.Quit();
	}

	public void NavToFileDialogue(string path) {
		OS.ShellShowInFileManager(ProjectSettings.GlobalizePath(path));
	}


}
