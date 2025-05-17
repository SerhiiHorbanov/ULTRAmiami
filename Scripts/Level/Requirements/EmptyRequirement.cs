namespace ULTRAmiami.Level.Requirements;

public partial class EmptyRequirement : CompletionRequirement
{
	public override bool IsCompleted()
		=> false;

	public override bool IsFailed()
		=> false;
}
