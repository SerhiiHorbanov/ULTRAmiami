using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Godot;
using ULTRAmiami.Level.Requirements;
using ULTRAmiami.Units;

namespace ULTRAmiami.Level;

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
		if (IsEverythingIsCompleted())
			EmitSignalOnCompleted();
	}
	
	private bool IsEverythingIsCompleted()
	{
		bool everythingIsCompleted = true;

		foreach (CompletionRequirement requirement in _requirements)
		{
			if (requirement.IsFailed())
			{
				Fail(requirement);
				return false;
			}
			
			everythingIsCompleted &= requirement.IsCompleted();
		}
		return everythingIsCompleted;
	}

	private void Fail(CompletionRequirement requirement)
	{
		_player.Die(new(Vector2.Zero, 0));
	}
}