using Godot;
using System;

public partial class GameCamera : Node2D {

	[Export] private Camera2D camera;

	public override void _Ready() {
		base._Ready();

		float zoom = GameManager.Instance.Settings.CameraZoom;
		camera.Zoom = new Vector2(zoom, zoom);

	}

}
