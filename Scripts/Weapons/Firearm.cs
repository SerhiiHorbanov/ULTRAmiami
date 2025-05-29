using Godot;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons.Projectiles;

namespace ULTRAmiami.Weapons;

public partial class Firearm : Weapon
{
    [Export] private PackedScene _bullet;
    
    protected override void Shoot(Vector2 shootingAt)
    {
        Node projectile = _bullet.Instantiate();
        projectile.MakeSiblingOf(GetParent());
        
        if (projectile is not IFirearmProjectile firearmProjectile)
        {
            GD.PrintErr("Projectile is not a IFirearmProjectile");
            return;
        }
        
        firearmProjectile.Initialize(GlobalPosition, shootingAt);
    }
}
