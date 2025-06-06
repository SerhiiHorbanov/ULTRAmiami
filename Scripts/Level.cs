using Godot;
using Godot.Collections;
using ULTRAmiami.Data;
using ULTRAmiami.UI;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class Level : Node
{
	[Export] private LevelInfo _info;

	[Export] private LevelCompletionMenu _completionMenu;
	
	[Export] private Array<Node> _nodesToFreeOnCompletion;
	[Export] private Unit _player;
	
	private readonly static StringName PlayNextLevel = "play next level";
	
	public override void _Ready()
	{
		if (_info is null)
			GD.PrintErr("Info is null");
	}

	private void OnCompleted()
	{
		if (_info is null)
			return;

		_player.GodMode = true;

		foreach (Node node in _nodesToFreeOnCompletion)
			node.QueueFree();
		
		_info.AddCompletion(PlayerScore.Current);
		_completionMenu.Display(_info, PlayerScore.Current);
	}

	private void LoadNextLevel()
	{
		_info.LoadNextLevel(this);
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!_info?.HasNextLevel ?? true)
			return;
		if (!_info.IsCompleted())
			return;
		
		if (@event.IsActionPressed(PlayNextLevel))
			_info.LoadNextLevel(this);
	}
}
