using Godot;
using System;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons.Projectiles;

namespace ULTRAmiami.Weapons;

public partial class Railgun : Weapon
{
	[Export] private PackedScene _projectileScene;
	
	protected override void Shoot(Vector2 shootingAt)
	{
		RailgunShot shot = _projectileScene.Instantiate<RailgunShot>();
		shot.MakeSiblingOf(GetParent());
		shot.Initialize(GlobalPosition, shootingAt, Unit);
		Unit.CallDeferred(Units.Unit.MethodName.DropWeapon);
	}
}
