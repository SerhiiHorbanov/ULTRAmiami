using Godot;

namespace ULTRAmiami.Requirements;

public partial class EmptyRequirement : CompletionRequirement
{
	public override Node InstantiateAndConnectToUI()
	{
		Label label = new();
		label.Text = "Empty requirement";
		return label;
	}

	public override bool IsCompleted()
		=> false;

	public override bool IsFailed()
		=> false;
}
