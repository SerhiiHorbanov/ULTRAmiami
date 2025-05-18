using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami.Requirements;

public partial class ReachAreaRequirement : CompletionRequirement
{
	[Export] private Array<Area2D> _areas;

	private bool _reached;
	
	public override void _Ready()
	{
		foreach (Area2D area in _areas)
		{
			area.BodyEntered += BodyEntered;
		}
	}

	private void BodyEntered(Node2D body)
	{
		if (body is not Unit unit)
			return;
		if (!unit.IsPlayer)
			return;
		
		_reached = true;
		UnsubscribeFromAreas();
	}
	
	private void UnsubscribeFromAreas()
	{
		foreach (Area2D area in _areas)
		{
			area.BodyEntered -= BodyEntered;
		}
	}

	public override bool IsCompleted()
		=> _reached;

	public override bool IsFailed()
		=> false;
}