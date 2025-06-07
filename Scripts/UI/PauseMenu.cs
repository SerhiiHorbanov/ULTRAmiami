using Godot;
using ULTRAmiami.UI.SettingsMenuUI;

namespace ULTRAmiami.UI;

public partial class PauseMenu : Control
{
	private readonly static StringName Escape = "escape";

	[Export] private SettingsMenu _settings;
	
	[Export] private PackedScene _sceneExitingTo;

	[Export] private bool _isPaused;

	[Signal]
	public delegate void OnExitingEventHandler();
	
	[Signal]
	public delegate void OnRestartLevelEventHandler();
	
	[Signal]
	public delegate void OnNextLevelEventHandler();
	
	public override void _Ready()
	{
		SetPaused(_isPaused);
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (_settings.Visible)
			return;
		
		if (@event.IsActionPressed(Escape))
		{
			SetPaused(!_isPaused);
		}
	}

	private void SetPaused(bool paused)
	{
		MouseVisibilityHandler.ChangeVisibility(paused);
		
		Visible = paused;
		_isPaused = paused;

		if (!paused)
			_settings.Close();
	}
	
	private void Resume()
		=> SetPaused(false);

	private void Settings()
	{
		_settings.Open();
	}
	
	private void RestartLevel()
		=> EmitSignalOnRestartLevel();

	private void NextLevel()
		=> EmitSignalOnNextLevel();
	
	private void Exit()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToPacked(_sceneExitingTo);
		EmitSignalOnExiting();
	}
}
