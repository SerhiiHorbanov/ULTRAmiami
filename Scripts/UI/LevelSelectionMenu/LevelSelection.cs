using Godot;
using Godot.Collections;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.LevelSelectionMenu;

public partial class LevelSelection : Control
{
	[Export] private Array<LevelInfo> _levels;

	[Export] private PackedScene _levelWidgetScene;
	[Export] private Node _widgetsParent;

	private readonly static StringName Escape = "escape";
	
	public override void _Ready()
	{
		foreach (LevelInfo level in _levels)
		{
			CreateWidgetForLevel(level);
		}
	}

	private void CreateWidgetForLevel(LevelInfo level)
	{
		LevelWidget widget = _levelWidgetScene.Instantiate<LevelWidget>();
		
		_widgetsParent.AddChild(widget);
		widget.Initialize(level);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed(Escape))
			return;

		Visible = false;
	}
}
