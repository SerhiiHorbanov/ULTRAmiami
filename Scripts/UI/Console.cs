using System.Collections.Generic;
using System.Linq;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.UI;

public partial class Console : Control
{
	[Export] private LineEdit _lineEdit;
	[Export] private RichTextLabel _text;

	[Export] private Control _disappearingPart;
	private bool _isOnScreen;
	
	private readonly static List<string> History = [];
	private int _currentCommandInHistoryIndex;

	private readonly static StringName ConsoleOpenClose = new("console open close");
	private readonly static StringName UITextCaretUp = new("ui_text_caret_up");
	private readonly static StringName UITextCaretDown = new("ui_text_caret_down");

	[Export(PropertyHint.MultilineText)] private string _helpText;

	[Export] private Unit _player;
	[Export(PropertyHint.File)] private string _scenesPath = "res://Scenes/";
	[Export] private Godot.Collections.Dictionary<string, PackedScene> _scenes;
	
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

	public void Echo(string text = "")
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
		_currentCommandInHistoryIndex = int.Clamp(_currentCommandInHistoryIndex, 0, History.Count);

		if (_currentCommandInHistoryIndex == History.Count)
		{
			_lineEdit.Text = "";
			return;
		}
		_lineEdit.Text = History[_currentCommandInHistoryIndex];
	}
	
	private void TryAddToHistory(string line)
	{
		if (History.Count == 0)
			History.Add(line);
		else if (History[^1] != line)
			History.Add(line);
	}
	
	public void EnterCommand(string line)
	{
		TryAddToHistory(line);
		_currentCommandInHistoryIndex = History.Count;

		ResolveCommand(line);
		
		_lineEdit.Clear();
	}

	private void ResolveCommand(string command)
	{
		string[] words = command.Split(' ');
		
		Echo(">>> " + command);
		
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
			case "spawn":
				Spawn(words[1], words.Skip(2).ToArray());
				break;
			case "scenes":
				ShowScenes();
				break;
			case "godmode":
				_player.GodMode = !_player.GodMode;
				break;
			default:
				Echo($"[color=red]Invalid command:[color=white] {command}");
				break;
		}
	}

	private void ShowScenes()
	{
		Echo("Scenes:");
		foreach ((string name, PackedScene scene) in _scenes)
		{
			Echo($"\t{name}:");
			Echo($"\t\tresource path: {scene.ResourcePath}");
			Echo();
		}
		
	}

	private void Spawn(string scene, string[] args)
	{
		if (_scenes.TryGetValue(scene, out PackedScene packedScene))
		{
			Spawn(packedScene, args);
			return;
		}

		if (!scene.EndsWith(".tscn"))
		{
			Echo($"[color=red]Couldn't find scene:[color=white] \"{scene}\"");
			return;
		}

		if (!scene.StartsWith(_scenesPath))
			scene = _scenesPath + scene;
		
		if (!ResourceLoader.Exists(scene))
		{
			Echo($"[color=red]Couldn't find scene resource:[color=white] \"{scene}\"");
			return;
		}
		
		Spawn(ResourceLoader.Load<PackedScene>(scene), args);
	}
	private void Spawn(PackedScene scene, string[] args)
	{
		Node instance = scene.Instantiate();
		GetTree().CurrentScene.AddChild(instance);
		Echo($"Spawned {instance.Name}");
	}
}
