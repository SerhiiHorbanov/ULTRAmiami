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
	[Export] private float _droppedRotationRandomness = 20f;
	
	[Export] private AudioStreamPlayer2D _shootingAudio;
	[Export] private AudioStreamPlayer2D _failingShotAudio;
	[Export] private AudioStreamPlayer2D _reloadingAudio;

	public Unit Unit { get; private set; }
	public Vector2 PointingAt;
	
	public event Action<int> OnAmmoChanged;
	public event Action OnReloadFinished;
	public event Action<int> OnShoot;
	public event Action OnUnitChanged;
	
	protected Weapon()
	{
		_ammo = _maxAmmo;
	}

	public bool IsAutomatic
		=> _isAutomatic;

	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	private ulong MicroSecondsSinceShot
		=> Time.GetTicksUsec() - _lastShotMicroSeconds;

	private bool IsDropped
		=> Unit is null;
	
	public Vector2 WeaponPosition
		=> IsDropped ? _dropped.Position : Unit.Position;
	
	public Vector2 GlobalWeaponPosition
		=> IsDropped ? _dropped.GlobalPosition : Unit.GlobalPosition;
	
	public Vector2 RelativePointingAt
		=> PointingAt - GlobalWeaponPosition;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public bool IsReloading
		=> !_reloadTimer.IsStopped();
	
	private float HalfSpreadRadians
		=> float.DegreesToRadians(_spread * 0.5f);
    
	private float HalfDroppedRotationRandomness 
		=> float.DegreesToRadians(_droppedRotationRandomness * 0.5f);


	public override void _Ready()
	{
		_droppedNotPickuppable.GetParent().RemoveChild(_droppedNotPickuppable);
		_reloadTimer.Timeout += FinishReloading;
	}

	public void TryAttachUnit(Unit unit, bool isPickUppable = true)
	{
		Unit = unit;
		_reloadingAudio.Stop();
		OnUnitChanged?.Invoke();

		if (unit is null)
		{
			Drop(isPickUppable);
			return;
		}

		Reparent(unit);
		Position = Vector2.Zero;
		RemoveChild(_dropped);
		SetMaxAmmo();
	}

	private void Drop(bool isPickUppable)
	{
		this.MakeSiblingOf(GetParent());
		Node2D dropped = isPickUppable ? _dropped : _droppedNotPickuppable;
		
		if (isPickUppable)
		{
			AddChild(_dropped);
		}
		else
		{
			_droppedNotPickuppable.MakeSiblingOf(this);
			_dropped.DetachWeapon();
			QueueFree();
		}
		
		dropped.GlobalPosition = GlobalPosition;
		dropped.Rotation = MyRandom.Range(HalfDroppedRotationRandomness);
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
	
	private void FinishReloading()
	{
		SetMaxAmmo();
		OnReloadFinished?.Invoke();
	}

	private void SetMaxAmmo()
	{
		_ammo = _maxAmmo;
		OnAmmoChanged?.Invoke(_ammo);
	}
	
	private void ShootAndDoRelatedProcesses()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		
		for (int i = 0 ; i < _shootsPerShot ; i++)
			ShootWithSpread();

		_ammo--;
		_shootingAudio?.Play();
		OnAmmoChanged?.Invoke(_ammo);
		OnShoot?.Invoke(_ammo);
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

	public bool EnoughTimeSinceLastShotToShootAgain()
	{
		return MicroSecondsSinceShot >= MicroSecondsBetweenShots;
	}
	
	protected abstract void Shoot(Vector2 shootingAt);
}
