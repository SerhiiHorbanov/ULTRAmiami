using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI;

public partial class LevelCompletionMenu : Control
{
	[Export] private RichTextLabel _levelNameLabel;
	
	[Export] private RichTextLabel _killsLabel;
	[Export] private RichTextLabel _bloodConsumedLabel;
	[Export] private RichTextLabel _timeLabel;
	[Export] private RichTextLabel _nextLevelLabel;
	
	[Export] private string _noNextLevelText;
	
	private LevelInfo _info;
	
	public void Display(LevelInfo levelInfo, PlayerScore playerScore)
	{
		Visible = true;
		
		_info = levelInfo;
		
		_levelNameLabel.Text = levelInfo.LevelName;
		_killsLabel.Text = playerScore.Kills.ToString();
		_bloodConsumedLabel.Text = playerScore.BloodConsumed.ToString("0.0");
		_timeLabel.Text = playerScore.TimeAlive.ToString("0.000");
		
		if (!levelInfo.HasNextLevel)
			_nextLevelLabel.Text = _noNextLevelText;
	}
}
