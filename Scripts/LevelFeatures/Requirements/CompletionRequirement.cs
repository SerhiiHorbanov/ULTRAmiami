using Godot;

namespace ULTRAmiami.Requirements;

public abstract partial class CompletionRequirement : Node
{
	public abstract bool IsCompleted();
	public abstract bool IsFailed();
}