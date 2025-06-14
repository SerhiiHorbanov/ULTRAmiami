using Godot;
using ULTRAmiami.Requirements;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class LevelTargetsUI : Control
{
	[Export] private RichTextLabel _targetsCounterLabel;
	[Export] private string _targetsCounterPrefix;
	[Export] private string _targetsCounterSuffix;
	[Export] private string _targetsCounterCompletedPrefix;
	
	private static LevelTargetsUI _instance;

	private int _displayedTargetsCompleted = -1;
	
	public override void _Ready()
	{
		_instance = this;
	}

	public override void _ExitTree()
	{
		_instance = null;
	}

	public static void SetTargetsCompleted(int targetsCompleted, int maxTargets)
		=> _instance.SetTargetsCompletedInternal(targetsCompleted, maxTargets);

	private void SetTargetsCompletedInternal(int targetsCompleted, int maxTargets)
	{
		if (_displayedTargetsCompleted == targetsCompleted)
			return;
		
		_displayedTargetsCompleted = targetsCompleted;
		UpdateTargetsCompletedLabel(maxTargets);
	}

	private void UpdateTargetsCompletedLabel(int maxTargets)
	{
		bool allCompleted = _displayedTargetsCompleted == maxTargets;

		string text;
		if (allCompleted)
			text = _targetsCounterPrefix + _targetsCounterCompletedPrefix + _displayedTargetsCompleted + '/' + maxTargets + _targetsCounterSuffix;
		else
			text = _targetsCounterPrefix + _displayedTargetsCompleted + '/' + maxTargets + _targetsCounterSuffix;
		
		_targetsCounterLabel.Text = text;
	}

	public static void AddLevelRequirement(CompletionRequirement requirement)
		=> _instance.AddLevelRequirementInternal(requirement);

	private void AddLevelRequirementInternal(CompletionRequirement requirement)
	{
		Node ui = requirement.InstantiateAndConnectToUI();
		
		if (ui is null)
			return;
		
		AddChild(ui);
	}
}
