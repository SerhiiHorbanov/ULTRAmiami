using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.Units;

public partial class PlayerTimeAliveScoreUpdater : Node
{
	public override void _Process(double delta)
	{
		PlayerScore.Current.AddTime((float)delta);
	}
}
