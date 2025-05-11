using Godot;
using ULTRAmiami.Resources;

namespace ULTRAmiami;

public partial class KeyBindsApplier : Node
{
	[Export] private KeyBinds _keyBinds;

	public override void _Ready()
	{
		_keyBinds.Apply();
	}
}
