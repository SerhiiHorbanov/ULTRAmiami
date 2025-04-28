using System;
using Godot;

namespace ULTRAmiami.UI;

public partial class TextOverTimePrinter : RichTextLabel
{
	[Export] private float _charactersPerSecond;

	private int _targetCharIndex;
	private int _currentCharIndex;

	private ulong _lastCharTime;
	
	private string _wholeText = string.Empty;

	private bool _wasFinished = true;
	
	private float TimePerCharacter
		=> 1 / _charactersPerSecond;
	
	[Signal]
	public delegate void OnFinishedEventHandler();

	public void Print(string text)
	{
		_wholeText += text;
	}

	public void ClearText()
	{
		_wholeText = string.Empty;
		Text = string.Empty;
	}

	public void ClearAndPrint(string text)
	{
		ClearText();
		Print(text);
	}
	
	public override void _Process(double delta)
	{
		UpdatePrintingText();
	}
	
	private void UpdatePrintingText()
	{
		bool isFinished = _currentCharIndex >= _wholeText.Length;
		
		if (isFinished)
		{
			_lastCharTime = Time.GetTicksUsec();
			
			if (!_wasFinished)
				EmitSignalOnFinished();
			_wasFinished = true;
			
			return;
		}
		_wasFinished = false;
		
		UpdateTargetCharIndex();
		AdvanceText(_targetCharIndex - _currentCharIndex);
	}

	private void UpdateTargetCharIndex()
	{
		float timeSinceLastChar = (Time.GetTicksUsec() - _lastCharTime) * 0.000001f;

		if (timeSinceLastChar < TimePerCharacter)
			return;
		
		_targetCharIndex = _currentCharIndex + (int)(timeSinceLastChar / TimePerCharacter);
		_targetCharIndex = int.Clamp(_targetCharIndex, 0, _wholeText.Length - 1);
		_lastCharTime = Time.GetTicksUsec();
	}

	private void AdvanceText(int charsAmount)
	{
		if (charsAmount <= 0)
			return;
		
		string toAdd = _wholeText.Substring(_currentCharIndex, charsAmount);
		Text += toAdd;
		
		_currentCharIndex += charsAmount;
	}
}
