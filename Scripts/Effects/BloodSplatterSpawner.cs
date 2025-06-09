using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami.Effects;

public partial class BloodSplatterSpawner : Node
{
	[Export] private Unit _unit;
	[Export] private Array<PackedScene> _splatterScenes = [];

	private void SpawnSplatter(Hit hit)
	{
		for (int i = 0; i < hit.SplatterAmount; i++)
			SpawnSingleSplatter(hit);
	}
	private void SpawnSingleSplatter(Hit hit)
	{
		PackedScene scene = _splatterScenes.PickRandom();
		BloodSplatter bloodSplatter = scene?.Instantiate<BloodSplatter>();

		if (bloodSplatter is null)
		{
			GD.PrintErr("BloodSplatter is null");
			return;
		}
		
		bloodSplatter.Initialize(_unit, hit);
	}
}
