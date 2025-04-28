using System;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.UI;

public enum RestartReason
{
	PlayerDeath,
	Restart,
}

public partial class GameplayRestartConsole : ColorRect
{
	[Export] private TextOverTimePrinter _consoleText;

	[Export(PropertyHint.File, "*.txt,")] private StringName _playerDeathRestartTextPath;
	private static string _playerDeathRestartText;
	
	[Signal]
	public delegate void OnAnimationStartedEventHandler();
	
	public override void _Ready()
	{
		ReadRestartTexts();
	}

	public void BeginDeathAnimation(Hit _)
		=> BeginDeathAnimation();
	
	public void BeginDeathAnimation()
	{
		EmitSignalOnAnimationStarted();
		_consoleText.ClearAndPrint(_playerDeathRestartText);
	}
	
	private void ReadRestartTexts()
	{
		FileAccess file = FileAccess.Open(_playerDeathRestartTextPath, FileAccess.ModeFlags.Read);
		_playerDeathRestartText = file.GetAsText();
		file.Close();
	}
}
