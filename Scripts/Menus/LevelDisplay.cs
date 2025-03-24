using Godot;
using System;

public partial class LevelDisplay : Control {

	[Export] private Label title;
	[Export] private Label stars;

	public void SetData(LevelData data) {
		title.Text = NameFormat(data.ScenePath);
		if (data.WasWon) {
			stars.Text = data.Score switch {
				1 => "★",
				2 => "★ ★",
				3 => "★ ★ ★",
				_ => "Error"
			};
		} else {
			stars.Text = "Unplayed";
		}
	}

	private static string NameFormat(string path) {
		string name = path.Split('/')[^1].Split('.')[0];

		RegEx regEx = RegEx.CreateFromString("[A-Z]");
		name = regEx.Sub(name, " $0", true);

		return name;
	}

}
