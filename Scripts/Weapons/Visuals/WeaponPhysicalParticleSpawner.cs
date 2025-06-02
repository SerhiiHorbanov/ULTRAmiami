using Godot;
using ULTRAmiami.Effects;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Visuals;

public partial class WeaponPhysicalParticleSpawner : Node2D
{
	[Export] private Weapon _weapon;
	[Export] private PackedScene _scene;

	private void Spawn()
	{
		if (_weapon is null)
			return;
		
		PhysicalParticle particle = _scene.Instantiate<PhysicalParticle>();
		particle.InitializeAsSiblingOfWithRotation(_weapon.Unit, _weapon.PointingInDirection.Angle());
	}
}
