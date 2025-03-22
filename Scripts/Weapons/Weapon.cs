using System;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node
{
	[Export] private bool _isAutomatic;
	[Export] private float _fireRate;

	private ulong _lastShotMicroSeconds;

	[Export] private int _ammo;
	[Export] private int _maxAmmo;
	
	public event Action<int> OnAmmoChanged;
	
	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	private ulong MicroSecondsSinceShot
		=> Time.GetTicksUsec() - _lastShotMicroSeconds;
	
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
		if (ReadyToShoot())
			ShootAndDoRelatedProcesses();
	}
	
	public void TryAutomaticShooting()
	{
		if (_isAutomatic && ReadyToShoot())
			ShootAndDoRelatedProcesses();
	}

	public void InstantReload()
	{
		_ammo = _maxAmmo;
		OnAmmoChanged?.Invoke(_ammo);
	}
	
	private void ShootAndDoRelatedProcesses()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		Shoot();
		_ammo--;
		
		OnAmmoChanged?.Invoke(_ammo);
	}

	private bool ReadyToShoot()
	{
		return MicroSecondsSinceShot >= MicroSecondsBetweenShots && _ammo != 0;
	}

	protected abstract void Shoot();
}
