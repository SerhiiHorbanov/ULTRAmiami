using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami.Effects;

public partial class BloodSplatterSpawner : Node
{
	[Export] private Unit _unit;
	[Export] private PackedScene _splatterSystemScene;
	[Export] private BloodSplatterSystem _splatterSystem;
	[Export] private bool _alwaysSpawnNewSplatterSystem;
	
	private void SpawnSplatter(Hit hit)
	{
		if (_splatterSystem is null || _alwaysSpawnNewSplatterSystem)
			SpawnSplatterSystem(hit);
		else
			_splatterSystem.AddSplattersFromHit(_unit, hit);

	}
	private void SpawnSplatterSystem(Hit hit)
	{
		BloodSplatterSystem splatter = _splatterSystemScene.Instantiate<BloodSplatterSystem>();
		
		if (splatter is null)
		{
			GD.PrintErr("BloodSplatter is null");
			return;
		}
		
		_splatterSystem = splatter;
		splatter.Initialize(_unit, hit);
	}
}
