using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelInfo : Resource
{
	[Export] private PackedScene _scene;
	[Export] private LevelInfo _nextLevel;
	private LevelCompletionInfo _completionInfo;
	
	public bool HasNextLevel 
		=> _completionInfo != null;
	
	public void LoadLevel(Node treeNode)
	{
		treeNode.GetTree().ChangeSceneToPacked(_scene);
	}
	
	public void LoadNextLevel(Node treeNode)
		=> _nextLevel.LoadLevel(treeNode);

	public bool IsCompleted()
		=> _completionInfo.IsCompleted;
}
