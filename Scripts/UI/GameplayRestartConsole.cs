using System;
using Godot;

namespace ULTRAmiami.UI;

public enum RestartReason
{
	PlayerDeath,
	Restart,
}

public partial class GameplayRestartConsole : RichTextLabel
{
	[Export] private float _charactersPerSecond;
	[Export(PropertyHint.File, "*.txt,")] private StringName _playerDeathRestartTextPath;
	private static string _playerDeathRestartText;

	private ulong _animationBeginning;
	
	private string _currentRestartText;
	
	private bool _isRunning;
	private int _currentCharIndex;
	private string _restartText;

	private string _restartReason;
	
	public override void _Ready()
	{
		if (_restartText is null)
			ReadRestartTexts();
	}

	public override void _Process(double delta)
	{
		if (!_isRunning)
			return;

		int targetCharacterIndex = GetTargetCharacterIndex();
		while (_currentCharIndex < targetCharacterIndex)
		{
			AdvanceText();
		}
	}

	private int GetTargetCharacterIndex()
		=> int.Min((int)((Time.GetTicksMsec() - _animationBeginning) * 0.001 *  _charactersPerSecond), _currentRestartText.Length - 1);

	private void AdvanceText()
	{
		Text += _playerDeathRestartText[_currentCharIndex];
		_currentCharIndex++;
	}

	public void BeginDeathAnimation()
	{
		_currentRestartText = _playerDeathRestartText;
		_currentCharIndex = 0;
		_animationBeginning = Time.GetTicksMsec();
		_isRunning = true;
	}
	
	private void ReadRestartTexts()
	{
		FileAccess file = FileAccess.Open(_playerDeathRestartTextPath, FileAccess.ModeFlags.Read);
		_playerDeathRestartText = file.GetAsText();
		file.Close();
	}
}
