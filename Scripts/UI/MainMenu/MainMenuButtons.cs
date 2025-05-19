using Godot;

namespace ULTRAmiami.UI.MainMenu;

public partial class MainMenuButtons : Node
{
	[Export] private Control _levelSelectionMenu;
	
	private void Play()
	{
		_levelSelectionMenu.Visible = true;
	}

	private void ExitGame()
	{
		GetTree().Quit();
	}
}
