using System.Collections.Generic;
using Godot;
using ULTRAmiami.Requirements;
using ULTRAmiami.UI.LevelRequirementsUI;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class LevelTarget : Node2D
{
	[Export] private Unit _player;
	private readonly List<CompletionRequirement> _requirements = [];
	private bool _isCompleted;

	public int TargetsCompleted {get; private set;}
	
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
		LevelTargetsUI.SetTargetsCompleted(TargetsCompleted, _requirements.Count);
	}
	
	private void ResolveCompletionAndFailure()
	{
		if (_isCompleted)
			return;
		
		bool everythingIsCompleted = true;
		CompletionRequirement failedRequirement = null;
		
		TargetsCompleted = 0;
		
		foreach (CompletionRequirement requirement in _requirements)
		{
			if (failedRequirement is null && requirement.IsFailed())
				failedRequirement = requirement;

			bool isCompleted = requirement.IsCompleted();
			everythingIsCompleted &= isCompleted;
			if (isCompleted)
				TargetsCompleted++;
		}

		if (failedRequirement is not null)
		{
			Fail(failedRequirement);
		}
		else if (everythingIsCompleted)
		{
			_isCompleted = true;
			EmitSignalOnCompleted();
		}
	}

	private void Fail(CompletionRequirement requirement)
	{
		_player.Die(new(Vector2.Zero, 0, new()));
	}
}