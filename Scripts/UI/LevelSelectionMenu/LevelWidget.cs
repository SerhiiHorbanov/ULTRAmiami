using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.LevelSelectionMenu;

public partial class LevelWidget : Node
{
	[Export] private Label _nameLabel;
	[Export] private Control _levelLockedOverlay;
	
	[Export] private Label _killsLabel;
	[Export] private Label _bloodConsumedLabel;
	[Export] private Label _timeLabel;
	
	private LevelInfo _levelInfo;
	private bool _isLocked;

	private PlayerScore Score
		=> _levelInfo.GetSavedCompletionScore();
	
	public void Initialize(LevelInfo levelInfo, bool isLocked)
	{
		_levelInfo = levelInfo;
		_isLocked = isLocked;
		
		_nameLabel.Text = levelInfo.LevelName;
		_levelLockedOverlay.Visible = isLocked;
		
		_timeLabel.Text = Score.TimeAlive.ToString("0.000");
		_killsLabel.Text = Score.Kills.ToString();
		_bloodConsumedLabel.Text = Score.BloodConsumed.ToString("0.0");
	}

	private void OnPressed()
	{
		if (_isLocked)
			return;
		
		_levelInfo.LoadLevel(this);
	}
}
