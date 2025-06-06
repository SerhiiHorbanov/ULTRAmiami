using Godot;
using System.IO;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelInfo : Resource
{
	[Export(PropertyHint.None, "Must be a legal file name before dot")] private string _saveName;
	[Export] private string _shownName;
	[Export(PropertyHint.File, "*.tscn")] private string _scenePath;
	[Export] private LevelInfo _nextLevel;
	private LevelCompletionInfo _completionInfo;

	private const string CompletionInfosSavePath = "user://LevelCompletionInfos/";
	
	public bool HasNextLevel 
		=> _nextLevel is not null;
	
	public LevelInfo NextLevel
		=> _nextLevel;
	
	public string LevelName
		=> _shownName;

	public PlayerScore GetMaxKillsScore()
	{
		EnsureCompletionNotNull();
		return _completionInfo?.MaxKillsScore;
	}

	public PlayerScore GetMaxBloodScore()
	{
		EnsureCompletionNotNull();
		return _completionInfo?.MaxBloodScore;
	}
	
	public PlayerScore GetBestTimeScore()
	{
		EnsureCompletionNotNull();
		return _completionInfo?.BestTimeScore;
	}
	
	public bool IsCompleted()
	{
		EnsureCompletionNotNull();
		return _completionInfo.IsCompleted;
	}
	
	public void LoadNextLevel(Node treeNode)
		=> _nextLevel.LoadLevel(treeNode);
	
	public void LoadLevel(Node treeNode)
	{
		PackedScene scene = (PackedScene)ResourceLoader.Load(_scenePath);
		treeNode.GetTree().ChangeSceneToPacked(scene);
	}

	public void AddCompletion(PlayerScore score)
	{
		EnsureCompletionNotNull();
		_completionInfo.AddCompletion(score);

		SaveCompletion();
	}
	
	private void SaveCompletion()
	{
		string globalPath = ProjectSettings.GlobalizePath(CompletionInfosSavePath);

		if (!Directory.Exists(globalPath))
			Directory.CreateDirectory(globalPath);
		
		ResourceSaver.Save(_completionInfo, GetOwnCompletionInfoFilePath());
	}

	public void EnsureCompletionNotNull()
	{
		if (_completionInfo is not null)
			return;
		
		string path = GetOwnCompletionInfoFilePath();

		if (ResourceLoader.Exists(path))
			_completionInfo = ResourceLoader.Load<LevelCompletionInfo>(path);
		else
			_completionInfo = new();
	}
	
	private string GetOwnCompletionInfoFilePath()
		=> CompletionInfosSavePath + _saveName + ".tres";
}
