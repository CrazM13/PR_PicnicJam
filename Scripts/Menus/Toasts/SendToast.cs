using Godot;
using System;

public partial class SendToast : Node {

	[Export] private string message;
	[Export] private Texture2D icon;

	[Export] private ToastContainer container;

	[Export] private bool sendOnStart = false;

	public override void _Ready() {
		base._Ready();

		if (sendOnStart) CallDeferred("SendNewToast");

	}

	public void SendNewToast() {
		container.AddToast(3, message, icon);
	}

}
