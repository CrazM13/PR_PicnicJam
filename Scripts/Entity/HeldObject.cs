using Godot;
using System;
using System.Collections.Generic;

public partial class HeldObject : CharacterBody2D {

	[Export] private Sprite2D sprite;

	private static int lastIndex = -1;
	private static int lastLastIndex = -1;

	public bool IsCollectable { get; set; } = true;
	public bool IsRunningPhysics { get; set; } = true;

	public override void _Ready() {
		base._Ready();

		List<string> unlockedContent = GameManager.Instance.GetAllUnlocked();

		RandomNumberGenerator rng = new RandomNumberGenerator();
		int index = rng.RandiRange(0, unlockedContent.Count - 1);

		while (index == lastIndex || index == lastLastIndex) {
			index = (index + 1) % unlockedContent.Count;
		}

		lastLastIndex = lastIndex;
		lastIndex = index;

		sprite.Texture = ResourceLoader.Load<Texture2D>($"res://Assets/Textures/Testing/PicnicContents/{unlockedContent[index]}.tres");
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);

		if (IsRunningPhysics) {
			Velocity += GetGravity() * (float) delta;

			MoveAndSlide();
		}
	}

}
