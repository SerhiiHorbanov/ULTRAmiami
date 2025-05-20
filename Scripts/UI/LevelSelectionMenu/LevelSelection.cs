using Godot;
using Godot.Collections;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.LevelSelectionMenu;

public partial class LevelSelection : Control
{
	[Export] private LevelInfo _firstLevel;

	[Export] private PackedScene _levelWidgetScene;
	[Export] private Node _widgetsParent;

	private readonly static StringName Escape = "escape";
	
	public override void _Ready()
	{
		LevelInfo current = _firstLevel;
		bool isLocked = false;
		while (current is not null)
		{
			CreateWidgetForLevel(current, isLocked);
			isLocked = !current.IsCompleted();
			current = current.NextLevel;
		}
	}

	private void CreateWidgetForLevel(LevelInfo level, bool isLocked)
	{
		LevelWidget widget = _levelWidgetScene.Instantiate<LevelWidget>();
		
		_widgetsParent.AddChild(widget);
		widget.Initialize(level, isLocked);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (!@event.IsActionPressed(Escape))
			return;

		Visible = false;
	}
}
