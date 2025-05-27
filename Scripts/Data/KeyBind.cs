using Godot;
using Godot.Collections;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class KeyBind : Resource
{
	[Export] public InputEventKey Key;
	[Export] public InputEventMouseButton MouseButton;
	
	public KeyBind(KeyBind original)
	{
		Key = original.Key;
		MouseButton = original.MouseButton;
	}
	
	public KeyBind()
	{ }

	public void TryApplying(StringName name)
	{
		if (!CanBeApplied())
			return;
		
		InputMap.ActionEraseEvents(name);
		if (Key is not null)
			InputMap.ActionAddEvent(name, Key);
		if (MouseButton is not null)
			InputMap.ActionAddEvent(name, MouseButton);
	}

	private bool CanBeApplied()
	{
		bool validKeyEvent = Key is not null;
		bool validMouseButton = MouseButton is not null;
		
		if (validKeyEvent)
			validKeyEvent &= Key.Keycode != Godot.Key.None;
		if (validMouseButton)
			validMouseButton &= MouseButton.ButtonIndex != Godot.MouseButton.None;
		
		return validMouseButton || validKeyEvent;
	}

	public static KeyBind CreateFromAction(StringName name)
	{
		Array<InputEvent> events = InputMap.ActionGetEvents(name);
		KeyBind keyBind = new();
		
		foreach (InputEvent @event in events)
		{
			if (keyBind.Key is not null && keyBind.MouseButton is not null)
				return keyBind;
			
			if (@event is InputEventKey key)
				keyBind.Key = key;
			if (@event is InputEventMouseButton button)
				keyBind.MouseButton = button;
		}
		
		return keyBind;
	}
}
