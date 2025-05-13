using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.Debug;

public partial class KeyBindsApplier : Node
{
	[Export] private KeyBinds _keyBinds;

	public override void _Ready()
	{
		_keyBinds.Apply();
	}
}
