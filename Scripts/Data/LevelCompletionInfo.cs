using Godot;

namespace ULTRAmiami.Data;

public partial class LevelCompletionInfo : Resource
{
	public bool IsCompleted { get; private set; }

	public void CompleteLevel()
		=> IsCompleted = true;
}
