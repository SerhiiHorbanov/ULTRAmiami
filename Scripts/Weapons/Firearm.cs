using Godot;
using ULTRAmiami.Weapons.Projectiles;

namespace ULTRAmiami.Weapons;

public partial class Firearm : Weapon
{
    [Export] private PackedScene _bullet;
    
    protected override void Shoot(Vector2 shootingAt)
    {
        Bullet projectile = _bullet.Instantiate<Bullet>();
        projectile.MakeSiblingOf(GetParent());
        projectile.Initialize(GlobalPosition, shootingAt);
    }
}
