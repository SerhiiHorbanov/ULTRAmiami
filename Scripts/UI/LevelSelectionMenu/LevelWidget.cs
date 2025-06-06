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

	[Export] private Button _maxKillsScoreButton;
	[Export] private Button _maxBloodScoreButton;
	[Export] private Button _bestTimeScoreButton;
	
	private LevelInfo _levelInfo;
	private bool _isLocked;

	public void Initialize(LevelInfo levelInfo, bool isLocked)
	{
		_levelInfo = levelInfo;
		_isLocked = isLocked;
		
		_nameLabel.Text = levelInfo.LevelName;
		_levelLockedOverlay.Visible = isLocked;
		
		DisplayScore(_levelInfo.GetBestTimeScore());
		_bestTimeScoreButton.SetPressed(true);
	}
	
	private void DisplayScore(PlayerScore score)
	{
		if (score is null)
			return;
		
		_timeLabel.Text = score.TimeAlive.ToString("0.000");
		_killsLabel.Text = score.Kills.ToString();
		_bloodConsumedLabel.Text = score.BloodConsumed.ToString("0.0");
	}

	private void OnPressed()
	{
		if (_isLocked)
			return;
		
		_levelInfo.LoadLevel(this);
	}

	private void SetMaxKillsScore()
	{
		DisplayScore(_levelInfo.GetMaxKillsScore());
		
		_maxKillsScoreButton.SetPressed(true);
		_maxBloodScoreButton.SetPressed(false);
		_bestTimeScoreButton.SetPressed(false);
	}
	
	private void SetMaxBloodScore()
	{
		DisplayScore(_levelInfo.GetMaxBloodScore());
		
		_maxKillsScoreButton.SetPressed(false);
		_maxBloodScoreButton.SetPressed(true);
		_bestTimeScoreButton.SetPressed(false);
	}
	
	private void SetBestTimeScore()
	{
		DisplayScore(_levelInfo.GetBestTimeScore());
		
		_maxKillsScoreButton.SetPressed(false);
		_maxBloodScoreButton.SetPressed(false);
		_bestTimeScoreButton.SetPressed(true);
	}
}
