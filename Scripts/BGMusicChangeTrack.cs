using Godot;
using System;

public partial class BGMusicChangeTrack : Node {

	[Export] private AudioStream track;

	public override void _Ready() {
		base._Ready();

		BackgroundMusic.Instance.ChangeTrack(track);

	}

}
