using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.Initialization;

public partial class PlayerScoreInitializer : Node
{
	public override void _Ready()
	{
		PlayerScore.Current = new();
		QueueFree();
	}
}
