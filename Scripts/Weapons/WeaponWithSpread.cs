using Godot;

namespace ULTRAmiami.Weapons;

public abstract partial class WeaponWithSpread : Weapon
{
    [Export] private float _spread;
	
    private float HalfSpreadRadians
        => float.DegreesToRadians(_spread * 0.5f);
    
    protected override void Shoot(Vector2 shootingAtWithoutSpread)
    {
        Vector2 shootingAt = AddSpread(shootingAtWithoutSpread);
        ShootAfterApplyingSpread(shootingAt);
    }

    protected abstract void ShootAfterApplyingSpread(Vector2 shooting);

    private Vector2 AddSpread(Vector2 shootingAt)
        => AddSpread(shootingAt, HalfSpreadRadians, HalfSpreadRadians);
	
    private Vector2 AddSpreadOnlyRight(Vector2 shootingDirection)
        => AddSpread(shootingDirection, 0, HalfSpreadRadians);
	
    private Vector2 AddSpreadOnlyLeft(Vector2 shootingDirection)
        => AddSpread(shootingDirection, HalfSpreadRadians, 0);
	
    private Vector2 AddSpread(Vector2 shootingAt, float maxDeviationToLeftRadians, float maxDeviationToRightRadians)
    {
        float deviation = MyRandom.Range(-maxDeviationToLeftRadians, maxDeviationToRightRadians);
		
        return shootingAt.RotateAround(Position, deviation);
    }
}
