using Godot;
using Godot.Collections;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class KeyBinds : Resource
{
	[Export] private Dictionary<string, KeyBind> _keyBinds = [];

	public void Apply()
	{
		foreach ((string name, KeyBind bind) in _keyBinds)
		{
			bind.TryApplying(name);
		}
	}
}
