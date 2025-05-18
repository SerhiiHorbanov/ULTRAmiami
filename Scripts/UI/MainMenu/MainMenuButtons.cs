using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.MainMenu;

public partial class MainMenuButtons : Node
{
	[Export] private LevelInfo _level;
	
	private void PlayLevel()
	{
		_level.LoadLevel(this);
	}

	private void ExitGame()
	{
		GetTree().Quit();
	}
}