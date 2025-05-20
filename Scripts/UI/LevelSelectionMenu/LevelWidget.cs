using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.LevelSelectionMenu;

public partial class LevelWidget : Node
{
	[Export] private Label _nameLabel;
	[Export] private Control _levelLockedOverlay;
	
	private LevelInfo _levelInfo;
	private bool _isLocked;
	
	public void Initialize(LevelInfo levelInfo, bool isLocked)
	{
		_levelInfo = levelInfo;
		_isLocked = isLocked;
		
		_nameLabel.Text = levelInfo.LevelName;
		_levelLockedOverlay.Visible = isLocked;
	}

	private void OnPressed()
	{
		if (_isLocked)
			return;
		
		_levelInfo.LoadLevel(this);
	}
}