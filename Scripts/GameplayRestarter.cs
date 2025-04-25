using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class GameplayRestarter : Timer
{
	public override void _Ready()
	{
		Timeout += RestartGameplay;
	}

	public void StartTimer(Hit _)
		=> StartTimer();
	public void StartTimer()
	{
		Start();
	}

	public void RestartGameplay()
	{
		GetTree().ReloadCurrentScene();
	}
}
