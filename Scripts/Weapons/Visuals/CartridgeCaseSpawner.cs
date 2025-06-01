using Godot;
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
		
		CartridgeCase cartridge = _cartridgeCase.Instantiate<CartridgeCase>();
		cartridge.Rotation = _weapon.PointingInDirection.Angle();
		cartridge.Initialize(_weapon.PointingInDirection.Angle());
		cartridge.MakeSiblingOf(_weapon.Unit);
		cartridge.GlobalPosition = _weapon.GlobalPosition;
	}
}
