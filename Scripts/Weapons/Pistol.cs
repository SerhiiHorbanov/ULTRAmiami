using Godot;
using ULTRAmiami.Weapons.Projectiles;

namespace ULTRAmiami.Weapons;

public partial class Pistol : Weapon
{
    [Export] private PackedScene _projectile;
    
    protected override void Shoot(Vector2 shootingAt)
    {
        Bullet projectile = _projectile.Instantiate<Bullet>();
        AddChild(projectile);
        
        projectile.Initialize(Position, shootingAt);
    }
}