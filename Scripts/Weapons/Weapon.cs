using System;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node
{
	[Export] private bool _isAutomatic;
	[Export] private float _fireRate;

	private ulong _lastShotMicroSeconds;

	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	public Unit Unit { private get; set; }

	public Vector2 Position
		=> Unit.Position;
	
	public Vector2 PointingAt { protected get; set; }
	
	public Vector2 RelativePointingAt
		=> PointingAt - Position;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public void TryStartShooting()
	{
		if (IsReloaded())
			ShootAndUpdateLastShot();
	}
	
	public void TryAutomaticShooting()
	{
		if (_isAutomatic && IsReloaded())
			ShootAndUpdateLastShot();
	}

	private void ShootAndUpdateLastShot()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		Shoot();
	}

	private bool IsReloaded()
	{
		ulong now = Time.GetTicksUsec();
		ulong microSecondsSinceShot = now - _lastShotMicroSeconds; 
		return microSecondsSinceShot >= MicroSecondsBetweenShots;
	}

	protected abstract void Shoot();
}
