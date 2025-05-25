using Godot;
using ULTRAmiami.Requirements;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class LevelTargetsUI : Control
{
	private static LevelTargetsUI _instance;
	
	public override void _Ready()
	{
		_instance = this;
	}

	public override void _ExitTree()
	{
		_instance = null;
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
