using System;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node2D
{
	[Export] private bool _isAutomatic;
	[Export] private float _fireRate;
	[Export] private float _spread;
	[Export] private uint _shootsPerShot = 1;
	
	private ulong _lastShotMicroSeconds;

	private int _ammo;
	[Export] private int _maxAmmo;

	[Export] private Timer _reloadTimer;

	[Export] private DroppedWeapon _dropped;
	[Export] private Node2D _droppedNotPickuppable;
	
	[Export] private AudioStreamPlayer2D _shootingAudio;
	[Export] private AudioStreamPlayer2D _failingShotAudio;
	[Export] private AudioStreamPlayer2D _reloadingAudio;
	
	private Unit _unit;
	public Vector2 PointingAt;
	
	public event Action<int> OnAmmoChanged;
	public event Action OnReloadFinished;
	
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
	
	public Vector2 WeaponPosition
		=> IsDropped ? _dropped.Position : _unit.Position;
	
	public Vector2 GlobalWeaponPosition
		=> IsDropped ? _dropped.GlobalPosition : _unit.GlobalPosition;
	
	public Vector2 RelativePointingAt
		=> PointingAt - GlobalWeaponPosition;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public bool IsReloading
		=> !_reloadTimer.IsStopped();
	
	private float HalfSpreadRadians
		=> float.DegreesToRadians(_spread * 0.5f);

	public override void _Ready()
	{
		_droppedNotPickuppable.GetParent().RemoveChild(_droppedNotPickuppable);
		_reloadTimer.Timeout += InstantReload;
	}

	public void TryAttachUnit(Unit unit, bool isPickUppable = true)
	{
		_unit = unit;

		if (unit is null)
		{
			Drop(isPickUppable);
			return;
		}

		Reparent(unit);
		Position = Vector2.Zero;
		RemoveChild(_dropped);
		InstantReload();
	}

	private void Drop(bool isPickUppable)
	{
		this.MakeSiblingOf(GetParent());
		
		if (isPickUppable)
		{
			AddChild(_dropped);
			_dropped.GlobalPosition = GlobalPosition;
			return;
		}
		
		_droppedNotPickuppable.MakeSiblingOf(this);
		_droppedNotPickuppable.GlobalPosition = GlobalPosition;
		_dropped.DetachWeapon();
		QueueFree();
	}

	public void TryStartShooting()
	{
		if (!EnoughTimeSinceLastShotToShootAgain())
			return;
		
		if (HasAmmo())
			ShootAndDoRelatedProcesses();
		else
			FailShotBecauseNoAmmo();
	}

	private void FailShotBecauseNoAmmo()
	{
		_failingShotAudio?.Play();
	}

	public void TryAutomaticShooting()
	{
		if (!EnoughTimeSinceLastShotToShootAgain() || !_isAutomatic)
			return;
		
		if (HasAmmo())
			ShootAndDoRelatedProcesses();
	}

	public bool HasAmmo()
		=> _ammo != 0;
	
	public void Reload()
	{
		if (IsReloading)
			return;
		
		_reloadTimer.Start();
		_reloadingAudio?.Play();
	}
	
	private void InstantReload()
	{
		_ammo = _maxAmmo;
		OnAmmoChanged?.Invoke(_ammo);
		OnReloadFinished?.Invoke();
	}
	
	private void ShootAndDoRelatedProcesses()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		
		for (int i = 0 ; i < _shootsPerShot ; i++)
			ShootWithSpread();

		_ammo--;
		_shootingAudio?.Play();
		OnAmmoChanged?.Invoke(_ammo);
	}

	private void ShootWithSpread()
	{
		Vector2 shootingAt = AddSpread(PointingAt);
		Shoot(shootingAt);
	}

	private Vector2 AddSpread(Vector2 shootingAt)
		=> AddSpread(shootingAt, HalfSpreadRadians, HalfSpreadRadians);
	
	private Vector2 AddSpreadOnlyRight(Vector2 shootingDirection)
		=> AddSpread(shootingDirection, 0, HalfSpreadRadians);
	
	private Vector2 AddSpreadOnlyLeft(Vector2 shootingDirection)
		=> AddSpread(shootingDirection, HalfSpreadRadians, 0);
	
	private Vector2 AddSpread(Vector2 shootingAt, float maxDeviationToLeftRadians, float maxDeviationToRightRadians)
	{
		float deviation = MyRandom.Range(-maxDeviationToLeftRadians, maxDeviationToRightRadians);
		Vector2 relativeShootingAt = shootingAt - GlobalWeaponPosition;
		
		return relativeShootingAt.Rotated(deviation) + GlobalWeaponPosition;
	}

	private bool EnoughTimeSinceLastShotToShootAgain()
	{
		return MicroSecondsSinceShot >= MicroSecondsBetweenShots;
	}
	
	protected abstract void Shoot(Vector2 shootingAt);
}
