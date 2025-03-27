using Godot;
using System;

public partial class ToastContainer : VBoxContainer {

	[Export] private PackedScene toastPrefab;
	[Export] private bool flip;

	public Toast AddToast(float duration, string message, Texture2D icon = null) {
		Toast toast = toastPrefab.Instantiate<Toast>();

		toast.SetMessage(duration, message, icon);
		toast.Flip = flip;

		AddChild(toast);

		return toast;
	}

	public Toast AddToast(Toast toast) {

		AddChild(toast);

		return toast;
	}

}
