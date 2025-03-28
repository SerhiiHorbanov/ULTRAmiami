using Godot;

namespace ULTRAmiami.Weapons;

public partial class Shotgun : Firearm
{
    [Export] private uint _bulletsPerShot;

    protected override void Shoot(Vector2 shootingAtWithoutSpread)
    {
        for(int i = 0; i < _bulletsPerShot; i++)
            base.Shoot(shootingAtWithoutSpread);
    }
}