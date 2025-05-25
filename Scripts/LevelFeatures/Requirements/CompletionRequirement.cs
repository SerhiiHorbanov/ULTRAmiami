using Godot;
using ULTRAmiami.UI.LevelRequirementsUI;

namespace ULTRAmiami.Requirements;

public abstract partial class CompletionRequirement : Node
{
	[Export] private PackedScene _requirementUI;

	public override void _Ready()
	{
		if (_requirementUI is not null)
			LevelTargetsUI.AddLevelRequirement(this);
	}

	public virtual Node InstantiateAndConnectToUI()
		=> null;
	protected Node InstantiateUI()
		=> _requirementUI?.Instantiate();

	public abstract bool IsCompleted();
	public abstract bool IsFailed();
}