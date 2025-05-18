using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelInfo : Resource
{
	[Export] private PackedScene _scene;
	[Export] private LevelInfo _nextLevel;
	private LevelCompletionInfo _completionInfo;
	
	public void LoadLevel(Node treeNode)
	{
		treeNode.GetTree().ChangeSceneToPacked(_scene);
	}

	public bool IsCompleted()
		=> _completionInfo.IsCompleted;
}
