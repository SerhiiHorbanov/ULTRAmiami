using Godot;
using Godot.Collections;

namespace ULTRAmiami.Resources;

[GlobalClass]
public partial class KeyBind : Resource
{
	[Export] private InputEventKey _key;
	[Export] private InputEventMouseButton _mouseButton;

	public void TryApplying(StringName name)
	{
		if (!CanBeApplied())
			return;
		
		InputMap.ActionEraseEvents(name);
		InputMap.ActionAddEvent(name, _key);
		InputMap.ActionAddEvent(name, _mouseButton);
	}

	private bool CanBeApplied()
	{
		bool validKeyEvent = _key is not null;
		bool validMouseButton = _mouseButton is not null;
		
		if (validKeyEvent)
			validKeyEvent &= _key.Keycode != Key.None;
		if (validMouseButton)
			validMouseButton &= _mouseButton.ButtonIndex != MouseButton.None;
		
		return validMouseButton || validKeyEvent;
	}
}
