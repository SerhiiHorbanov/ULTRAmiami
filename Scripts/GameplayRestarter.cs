using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class GameplayRestarter : Node
{
	private readonly static StringName Restart = "restart";
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(Restart))
			RestartGameplay();
	}

	public void RestartGameplay()
	{
		GetTree().ReloadCurrentScene();
	}
}
