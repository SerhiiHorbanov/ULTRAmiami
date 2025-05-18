using System.Collections.Generic;
using Godot;
using ULTRAmiami.Requirements;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class LevelTarget : Node2D
{
	[Export] private Unit _player;
	private readonly List<CompletionRequirement> _requirements = [];

	[Signal]
	public delegate void OnCompletedEventHandler();
	
	public override void _Ready()
	{
		foreach (Node node in GetChildren())
		{
			if (node is not CompletionRequirement requirement)
				continue;
			
			_requirements.Add(requirement);
		}
	}

	public override void _Process(double delta)
	{
		ResolveCompletionAndFailure();
	}
	
	private void ResolveCompletionAndFailure()
	{
		bool everythingIsCompleted = true;

		foreach (CompletionRequirement requirement in _requirements)
		{
			if (requirement.IsFailed())
			{
				Fail(requirement);
				return;
			}
			
			everythingIsCompleted &= requirement.IsCompleted();
		}
		
		if (everythingIsCompleted)
			EmitSignalOnCompleted();
	}

	private void Fail(CompletionRequirement requirement)
	{
		_player.Die(new(Vector2.Zero, 0));
	}
}