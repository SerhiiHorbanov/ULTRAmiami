using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class AreaTrigger : Area2D
{
	[Export] private bool _activatedOnce = true;
	[Export] private bool _activatedOnlyByPlayer = true;
	
	[Signal]
	public delegate void OnTriggeringEventHandler();

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not Unit unit)
			return;
		
		if (_activatedOnlyByPlayer)
		{
			if (!unit.IsPlayer)
				return;
		}
		
		Trigger();
	}
	
	private void Trigger()
	{
		if (_activatedOnce)
			BodyEntered -= OnBodyEntered;
		
		EmitSignalOnTriggering();
	}
}
