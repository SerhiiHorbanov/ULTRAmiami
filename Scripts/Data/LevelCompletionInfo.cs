using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelCompletionInfo : Resource
{
	[Export] public bool IsCompleted { get; private set; }

	[Export] private PlayerScore _score;
	
	public LevelCompletionInfo()
	{ }
	
	public LevelCompletionInfo(PlayerScore score)
	{
		_score = score;
	}

	public void CompleteLevel()
		=> IsCompleted = true;
}
