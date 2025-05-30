using Godot;

namespace ULTRAmiami;

public partial class TriggerableDoor : Node2D
{
	[Export] private int _triggersNeeded;

	[Export] private AnimationPlayer _animationPlayer;
	[Export] private StringName _baseStateAnimationName;
	[Export] private StringName _changingStateAnimationName;

	public override void _Ready()
	{
		_animationPlayer.Play(_baseStateAnimationName);
	}

	public void Trigger()
	{
		if (_triggersNeeded == 0)
			return;
		
		_triggersNeeded--;

		if (_triggersNeeded == 0)
			ChangeState();
	}

	public void ChangeState()
	{
		_triggersNeeded = 0;
		_animationPlayer.Play(_changingStateAnimationName);
	}
}
