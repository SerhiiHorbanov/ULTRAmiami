using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ULTRAmiami.UI;

public partial class Console : Control
{
	[Export] private LineEdit _lineEdit;
	[Export] private RichTextLabel _text;

	[Export] private Control _disappearingPart;
	private bool _isOnScreen;
	
	
	private readonly List<string> _history = [];
	private int _currentCommandInHistoryIndex;

	private readonly static StringName ConsoleOpenClose = new("console open close");
	private readonly static StringName UITextCaretUp = new("ui_text_caret_up");
	private readonly static StringName UITextCaretDown = new("ui_text_caret_down");

	[Export(PropertyHint.MultilineText)] private string _helpText;
	
	public override void _Ready()
	{
		_lineEdit.TextSubmitted += EnterCommand;
		_disappearingPart.Visible = _isOnScreen;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(ConsoleOpenClose))
			SwitchIsOnScreen();
		
		if (!_isOnScreen)
			return;
		
		if (Input.IsActionJustPressed(UITextCaretUp))
			GoUpHistory();
		else if (Input.IsActionJustPressed(UITextCaretDown))
			GoDownHistory();
	}

	private void SwitchIsOnScreen()
	{
		_isOnScreen = !_isOnScreen;
		_disappearingPart.Visible = _isOnScreen;
	}

	public void Echo(string text)
	{
		_text.AppendText(text);
		_text.AppendText("\n");
	}

	public void Clear()
	{
		_text.Text = "";
	}
	
	private void GoUpHistory()
		=> MoveAlongHistory(-1);

	private void GoDownHistory()
		=> MoveAlongHistory(1);

	private void MoveAlongHistory(int delta)
	{
		_currentCommandInHistoryIndex += delta;
		_currentCommandInHistoryIndex = int.Clamp(_currentCommandInHistoryIndex, 0, _history.Count);

		if (_currentCommandInHistoryIndex == _history.Count)
		{
			_lineEdit.Text = "";
			return;
		}
		_lineEdit.Text = _history[_currentCommandInHistoryIndex];
	}
	
	private void TryAddToHistory(string line)
	{
		if (_history.Count == 0)
			_history.Add(line);
		else if (_history[^1] != line)
			_history.Add(line);
	}
	
	public void EnterCommand(string line)
	{
		TryAddToHistory(line);
		_currentCommandInHistoryIndex = _history.Count;

		ResolveCommand(line);
		
		_lineEdit.Clear();
	}

	private void ResolveCommand(string command)
	{
		string[] words = command.Split(' ');
		
		Echo(command);
		
		switch (words[0])
		{
			case "echo":
				Echo(string.Join(' ', words.Skip(1)));
				break;
			case "cls":
				Clear();
				break;
			case "help":
				Echo(_helpText);
				break;
			default:
				Echo("[color=red]Invalid command: [color=white]" + command);
				break;
		}
	}
}
