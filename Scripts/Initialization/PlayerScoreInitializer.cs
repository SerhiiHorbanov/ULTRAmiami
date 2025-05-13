using Godot;

namespace ULTRAmiami;

public partial class PlayerScoreInitializer : Node
{
	public override void _Ready()
	{
		PlayerScore.Current = new();
		QueueFree();
	}
}
