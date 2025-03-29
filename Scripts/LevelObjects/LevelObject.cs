using Godot;
using Godot.Collections;
using System;

public partial class LevelObject : Node2D {

	private static uint NEXT_ID = 0;
	public uint ID { get; private set; }

	private enum GateMode {
		OR = 0,
		AND = 1,
		NOR = 2,
		NAND = 3,
		XOR = 4,
		XNOR = 5
	}

	[Signal] public delegate void PowerChangeEventHandler(LevelObject levelObject);

	[Export] private bool invertPower = false;
	[Export] private bool toggleMode = false;
	[Export] private GateMode logicMode = GateMode.OR;

	public bool IsPowered { get; private set; }

	protected Dictionary<uint, bool> PowerSources { get; private set; }

	public override void _Ready() {
		ID = NEXT_ID++;
		base._Ready();

		PowerSources ??= new Dictionary<uint, bool>();

		IsPowered = invertPower;
		UpdatePowerState();
	}

	public void SetPower(bool powered, uint id) {
		PowerSources ??= new Dictionary<uint, bool>();

		if (PowerSources.ContainsKey(id)) {
			PowerSources[id] = powered;
		} else {
			PowerSources.Add(id, powered);
		}

		UpdatePowerState();
	}

	private void UpdatePowerState() {
		bool oldPower = IsPowered;

		bool[] sources = new bool[PowerSources.Count];
		PowerSources.Values.CopyTo(sources, 0);
		bool powered = logicMode switch {
			GateMode.OR => TryOr(sources),
			GateMode.AND => TryAnd(sources),
			GateMode.NOR => !TryOr(sources),
			GateMode.NAND => !TryAnd(sources),
			GateMode.XOR => TryXor(sources),
			GateMode.XNOR => !TryXor(sources),
			_ => false
		};

		if (invertPower) powered = !powered;

		if (toggleMode) {
			if (powered) IsPowered = !IsPowered;
		} else {
			IsPowered = powered;
		}

		if (oldPower != IsPowered) {
			EmitSignal(SignalName.PowerChange, this);
		}
	}

	private static bool TryOr(bool[] bools) {
		foreach (bool b in bools) {
			if (b) return true;
		}

		return false;
	}

	private static bool TryAnd(bool[] bools) {
		if (bools.Length == 0) return false;

		foreach (bool b in bools) {
			if (!b) return false;
		}

		return true;
	}

	private static bool TryXor(bool[] bools) {
		bool retFlag = false;

		foreach (bool b in bools) {
			if (b && !retFlag) retFlag = true;
			else if (b && retFlag) return false;
		}

		return retFlag;
	}

}
