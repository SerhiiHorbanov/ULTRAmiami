using Godot;

namespace ULTRAmiami;

public partial class TriggerableDoor : Node2D
{
	[Export] private int _triggersNeeded;

	[Export] private AnimationPlayer _animationPlayer;
	[Export] private StringName _openingAnimationName;
	
	public void Trigger()
	{
		if (_triggersNeeded == 0)
			return;
		
		_triggersNeeded--;

		if (_triggersNeeded == 0)
			Open();
	}

	public void Open()
	{
		_triggersNeeded = 0;
		_animationPlayer.Play(_openingAnimationName);
	}
}
