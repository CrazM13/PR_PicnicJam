using Godot;
using System;

public partial class ToastContainer : VBoxContainer {

	[Export] private PackedScene toastPrefab;

	public void AddToast(float duration, string message, Texture2D icon = null) {
		Toast toast = toastPrefab.Instantiate<Toast>();

		toast.SetMessage(duration, message, icon);

		AddChild(toast);
	}

}
