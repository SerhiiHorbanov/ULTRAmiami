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
	[Export] private RichTextLabel _consoleText;
	[Export] private float _charactersPerSecond;
	
	[Export] private Gradient _gradient;
	[Export] private float _timeForGradient;
	
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
		
		UpdateGradient();

		int targetCharacterIndex = GetTargetCharacterIndex();
		while (_currentCharIndex < targetCharacterIndex)
		{
			AdvanceText();
		}
	}
	private void UpdateGradient()
	{
		float t = (Time.GetTicksMsec() - _animationBeginning) * 0.001f / _timeForGradient;
		Color = _gradient.Sample(t);
	}

	private int GetTargetCharacterIndex()
		=> int.Min((int)((Time.GetTicksMsec() - _animationBeginning) * 0.001f *  _charactersPerSecond), _currentRestartText.Length - 1);

	private void AdvanceText()
	{
		_consoleText.Text += _playerDeathRestartText[_currentCharIndex];
		_currentCharIndex++;
	}

	public void BeginDeathAnimation(Hit _)
		=> BeginDeathAnimation();
	
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
