using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Level.Requirements;

public partial class SpecificUnitKillRequirement : CompletionRequirement
{
	private bool _isDead;

	[Export] private Unit _target;

	public override void _Ready()
	{
		_target.OnDeath += OnDeath;
	}

	private void OnDeath(Hit hit)
	{
		_isDead = true;
		_target.OnDeath -= OnDeath;
	}

	public override bool IsCompleted()
		=> _isDead;

	public override bool IsFailed()
		=> false;
}
