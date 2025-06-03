using Godot;

namespace ULTRAmiami.UI.MainMenu;

public partial class MainMenuButtons : Node
{
	[Export] private Control _levelSelectionMenu;
	[Export(PropertyHint.File, "*.tscn")] private string _infiniteModeScenePath;
	
	private void SelectLevel()
	{
		_levelSelectionMenu.Visible = true;
	}

	private void PlayInfinite()
	{
		GetTree().ChangeSceneToFile(_infiniteModeScenePath);
	}

	private void ExitGame()
	{
		GetTree().Quit();
	}
}
