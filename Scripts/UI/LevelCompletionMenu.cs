using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI;

public partial class LevelCompletionMenu : Control
{
	[Export] private RichTextLabel _levelNameLabel;
	
	[Export] private RichTextLabel _killsLabel;
	[Export] private RichTextLabel _bloodConsumedLabel;
	[Export] private RichTextLabel _nextLevelLabel;
	
	[Export] private string _noNextLevelText;
	
	private LevelInfo _info;
	
	public void Display(LevelInfo levelInfo, PlayerScore playerScore)
	{
		Visible = true;
		
		_info = levelInfo;
		
		_levelNameLabel.Text = levelInfo.LevelName;
		_killsLabel.Text = playerScore.Kills.ToString();
		_bloodConsumedLabel.Text = float.Round(playerScore.BloodConsumed, 1).ToString();
		
		if (!levelInfo.HasNextLevel)
			_nextLevelLabel.Text = _noNextLevelText;
	}
}
