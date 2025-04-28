using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class GameplayRestarter : Node
{
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("restart"))
			RestartGameplay();
	}

	public void RestartGameplay()
	{
		GetTree().ReloadCurrentScene();
	}
}
