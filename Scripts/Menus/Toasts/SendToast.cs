using Godot;
using System;

public partial class SendToast : Node {

	[Export] protected string Message { get; set; }
	[Export] protected Texture2D Icon { get; set; }

	[Export] protected ToastContainer Container { get; set; }

	[Export] private bool sendOnStart = false;

	public override void _Ready() {
		base._Ready();

		if (sendOnStart) CallDeferred("SendNewToast");

	}

	public virtual Toast SendNewToast() {
		return Container.AddToast(3, Message, Icon);
	}

}
