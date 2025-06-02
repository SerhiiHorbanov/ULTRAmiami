using Godot;
using ULTRAmiami.Effects;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Visuals;

public partial class CartridgeCaseSpawner : Node2D
{
	[Export] private Weapon _weapon;
	[Export] private PackedScene _cartridgeCase;

	private void SpawnCartridgeCase()
	{
		if (_weapon is null)
			return;
		
		PhysicalParticle cartridge = _cartridgeCase.Instantiate<PhysicalParticle>();
		cartridge.InitializeAsSiblingOfWithRotation(_weapon.Unit, _weapon.PointingInDirection.Angle());
	}
}
