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

	[Export] private Node2D _dropped;
	
	public event Action<int> OnAmmoChanged;
	
	[Export] private Unit _unit;
	
	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	private ulong MicroSecondsSinceShot
		=> Time.GetTicksUsec() - _lastShotMicroSeconds;

	public Vector2 Position
		=> _unit.Position;
	
	public Vector2 PointingAt { protected get; set; }
	
	public Vector2 RelativePointingAt
		=> PointingAt - Position;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public override void _Ready()
	{
		SetUnit(_unit);
	}

	public void SetUnit(Unit unit)
	{
		if (unit is null)
			Drop();
		else
			PickedUp(unit);
	}
	
	public void PickedUp(Unit unit)
	{
		CallDeferred(Node.MethodName.Reparent, unit);
		InstantReload();
		
		RemoveChild(_dropped);
		
		_unit = unit;
	}

	public void Drop()
	{
		AddChild(_dropped);
		
		_dropped.Position = _unit.Position;
		this.MakeSiblingOf(_unit);
		_unit = null;
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

	private void InstantReload()
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
