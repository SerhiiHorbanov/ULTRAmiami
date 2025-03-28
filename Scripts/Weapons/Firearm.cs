using Godot;
using ULTRAmiami.Weapons.Projectiles;

namespace ULTRAmiami.Weapons;

public partial class Firearm : WeaponWithSpread
{
    [Export] private PackedScene _bullet;

    protected override void ShootAfterApplyingSpread(Vector2 shootingAt)
    {
        Bullet projectile = _bullet.Instantiate<Bullet>();
        AddChild(projectile);
        
        projectile.Initialize(Position, shootingAt);
    }
}
