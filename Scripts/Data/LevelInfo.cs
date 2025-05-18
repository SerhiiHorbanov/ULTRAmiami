using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelInfo : Resource
{
	[Export(PropertyHint.File)] private string _scenePath;
	[Export] private LevelInfo _nextLevel;
	private LevelCompletionInfo _completionInfo;
	
	public bool HasNextLevel 
		=> _nextLevel is not null;
	
	public void LoadLevel(Node treeNode)
	{
		PackedScene scene = (PackedScene)ResourceLoader.Load(_scenePath);
		treeNode.GetTree().ChangeSceneToPacked(scene);
	}
	
	public void LoadNextLevel(Node treeNode)
		=> _nextLevel.LoadLevel(treeNode);

	public bool IsCompleted()
		=> _completionInfo.IsCompleted;
}
