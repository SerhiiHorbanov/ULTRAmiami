using Godot;
using Godot.Collections;
using ULTRAmiami.UI.LevelRequirementsUI;
using ULTRAmiami.Units;

namespace ULTRAmiami.Requirements;

public partial class ReachAreaRequirement : CompletionRequirement
{
	[Export] private string _areasName; 
	[Export] private Array<Area2D> _areas;

	private bool _reached;

	[Export(PropertyHint.Layers2DPhysics)] private uint _mask;
	
	[Signal]
	public delegate void AreaReachedEventHandler();
	
	public override void _Ready()
	{
		base._Ready();
		
		foreach (Area2D area in _areas)
		{
			area.CollisionMask |= _mask;
			area.BodyEntered += BodyEntered;
		}
	}

	public override Node InstantiateAndConnectToUI()
	{
		Node node = InstantiateUI();

		if (node is not ReachAreaRequirementUI ui)
		{
			GD.PrintErr("Instantiated node is not a ReachAreaRequirementUI");
			return null;
		}
		
		ui.Initialize(_areasName);
		AreaReached += ui.OnAreaReached;
		return ui;
	}

	private void BodyEntered(Node2D body)
	{
		if (body is not Unit unit)
			return;
		if (!unit.IsPlayer)
			return;
		
		_reached = true;
		EmitSignalAreaReached();
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