using Godot;
using ULTRAmiami.UI.LevelRequirementsUI;
using ULTRAmiami.Units;

namespace ULTRAmiami.Requirements;

public partial class SpecificUnitKillRequirement : CompletionRequirement
{
	private bool _isDead;

	[Export] private Unit _target;
	[Export] private string _targetName;

	private SpecificUnitKillRequirementUI _ui;
	
	public override Node InstantiateAndConnectToUI()
	{
		Node node = InstantiateUI();

		if (node is not SpecificUnitKillRequirementUI ui)
		{
			GD.PrintErr("Instantiated node is not a SpecificUnitKillRequirementUI");
			return null;
		}
		
		_ui = ui;
		_ui.Initialize(_targetName);
		return node;
	}

	public override void _Ready()
	{
		base._Ready();
		
		if (_target is null)
		{
			_isDead = true;
			return;
		}
		
		_target.OnDeath += OnDeath;
	}

	private void OnDeath(Hit hit)
	{
		_isDead = true;
		_target.OnDeath -= OnDeath;
		_ui?.ChangeTextToCompletedVersion();
	}

	public override bool IsCompleted()
		=> _isDead;

	public override bool IsFailed()
		=> false;
}
