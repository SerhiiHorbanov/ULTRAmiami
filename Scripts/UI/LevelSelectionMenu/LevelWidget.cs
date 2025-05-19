using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.LevelSelectionMenu;

public partial class LevelWidget : Node
{
	[Export] private Label _nameLabel;

	private LevelInfo _levelInfo;
	
	public void Initialize(LevelInfo levelInfo)
	{
		_levelInfo = levelInfo;
		
		_nameLabel.Text = levelInfo.LevelName;
	}

	private void LoadLevel()
		=> _levelInfo.LoadLevel(this);
}