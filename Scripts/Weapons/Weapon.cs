using System;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node
{
	[Export] private bool _isAutomatic;
	[Export] private float _fireRate;
	
	private ulong _lastShotMicroSeconds;

	private int _ammo;
	[Export] private int _maxAmmo;

	[Export] private float _reloadTimeSeconds;
	private Timer _reloadTimer;

	[Export] private DroppedWeapon _dropped;
	
	private Unit _unit;
	
	public event Action<int> OnAmmoChanged;

	protected Weapon()
	{
		_ammo = _maxAmmo;
	}

	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	private ulong MicroSecondsSinceShot
		=> Time.GetTicksUsec() - _lastShotMicroSeconds;

	private bool IsDropped
		=> _unit is null;
	
	public Vector2 Position
		=> IsDropped ? _dropped.Position : _unit.Position;
	
	public Vector2 PointingAt { protected get; set; }
	
	public Vector2 RelativePointingAt
		=> PointingAt - Position;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public bool IsReloading
		=> _reloadTimer is not null;
	
	public void TryAttachUnit(Unit unit, bool isPickUppable = true)
	{
		Unit prevUnit = _unit;
		_unit = unit;

		if (unit is null)
		{
			Drop(isPickUppable, prevUnit.Position);
			return;
		}
		
		RemoveChild(_dropped);
		InstantReload();
	}

	private void Drop(bool isPickUppable, Vector2 position)
	{
		_dropped.Position = position;
		
		if (isPickUppable)
		{
			AddChild(_dropped);
			return;
		}
			
		_dropped.MakeSiblingOf(this);
		_dropped.DetachWeapon();
		QueueFree();
	}

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

	public bool HasAmmo()
		=> _ammo != 0;
	
	public void Reload()
	{
		if (!IsReloading)
			StartReloadTimer();
	}

	private void StartReloadTimer()
	{
		_reloadTimer = new();
		
		AddChild(_reloadTimer);
		_reloadTimer.Timeout += InstantReload;
		_reloadTimer.Start(_reloadTimeSeconds);
		_reloadTimer.OneShot = true;
	}
	
	private void InstantReload()
	{
		_ammo = _maxAmmo;
		OnAmmoChanged?.Invoke(_ammo);
		
		_reloadTimer?.QueueFree();
		_reloadTimer = null;
	}
	
	private void ShootAndDoRelatedProcesses()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		
		Shoot(PointingAt);
		_ammo--;
		
		OnAmmoChanged?.Invoke(_ammo);
	}
	
	private bool ReadyToShoot()
	{
		return MicroSecondsSinceShot >= MicroSecondsBetweenShots && _ammo != 0;
	}

	protected abstract void Shoot(Vector2 shootingAt);
}
