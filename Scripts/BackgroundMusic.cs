using Godot;
using System;

public partial class BackgroundMusic : Node {

	#region Singleton
	public static BackgroundMusic Instance { get; set; }
	public override void _EnterTree() {
		base._EnterTree();
		Instance = this;
		GameManager _ = GameManager.Instance;
	}
	#endregion

	private string currentTrack;

	[Export] private AudioStreamPlayer mainTrack;
	[Export] private AudioStreamPlayer backupTrack;

	public override void _Ready() {
		base._Ready();
		currentTrack = mainTrack.Stream.ResourcePath;
	}

	public void ChangeTrack(AudioStream track) {
		if (currentTrack != track.ResourcePath) {
			currentTrack = track.ResourcePath;

			backupTrack.Stream = mainTrack.Stream;
			backupTrack.Play(mainTrack.GetPlaybackPosition());

			mainTrack.Stream = track;
			mainTrack.Play();

			mainTrack.VolumeLinear = 0;
			backupTrack.VolumeLinear = 1;
		}
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (backupTrack.VolumeLinear > 0) {
			backupTrack.VolumeLinear = Mathf.MoveToward(backupTrack.VolumeLinear, 0, (float) delta);
			mainTrack.VolumeLinear = Mathf.MoveToward(mainTrack.VolumeLinear, 1, (float) delta);

			if (backupTrack.VolumeLinear == 0) {
				backupTrack.Stop();
			}
		}

	}

}
