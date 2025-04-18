using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PistolEnemyUnitController : AIUnitController
{
	[Export] public Unit TargetUnit;

	[Export] private float _tooCloseToTargetDistance;
	[Export] private float _tooFarFromTargetDistance;
	[Export] private Vector2 _wayPointRandomizationRange;
	[Export] private Timer _shootingTimer;

	private Weapon _weaponToUnsubscribeFrom;
	
	private float TooCloseToTargetDistanceSquared
		=> _tooCloseToTargetDistance * _tooCloseToTargetDistance;
	private float TooFarFromTargetDistanceSquared
		=> _tooFarFromTargetDistance * _tooFarFromTargetDistance;

	private float DistanceSquaredToTarget
		=> Unit.Position.DistanceSquaredTo(TargetUnit.Position);
	
	private float DistanceToTarget
		=> Unit.Position.DistanceTo(TargetUnit.Position);
	
	private Vector2 DirectionToTarget
		=> (TargetUnit.Position - Unit.Position).Normalized();
	
	private Vector2 DirectionFromTarget
		=> -DirectionToTarget;
	
	public override void _Ready()
	{
		base._Ready();
		
		if (TargetUnit is not null)
			TargetUnit.OnDeath += StopTargetUnit;

		PlayerNoticed += SetTargetUnit;
		PlayerNoticed += _ => _shootingTimer.Start();
		
		_shootingTimer.Timeout += TryShooting;
	}
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (TargetUnit is null || Unit is null)
			return;

		if (ShouldReload())
			ReloadWeapon();

		SetNewWayPointIfShould();
		
		UpdatePointingAt();
	}

	public override void _ExitTree()
	{
		SetTargetUnitToNull();
	}

	private void TryShooting()
		=> Weapon?.TryStartShooting();

	private void ReloadWeapon()
	{
		Weapon.Reload();
		_weaponToUnsubscribeFrom = Weapon;
		Weapon.OnReloadFinished += OnReloadFinished;
		Unit.OnDeath += UnsubscribeFromWeaponReload;
		_shootingTimer.Stop();
	}

	private void UnsubscribeFromWeaponReload()
	{
		_weaponToUnsubscribeFrom.OnReloadFinished -= OnReloadFinished;
	}
	
	private void OnReloadFinished()
	{
		if (TargetUnit is not null)
			_shootingTimer.Start();
		Weapon.OnReloadFinished -= OnReloadFinished;
		Unit.OnDeath -= UnsubscribeFromWeaponReload;
	}

	private void SetNewWayPointIfShould()
	{
		if (IsGoingToWayPoint)
			return;
		if (DistanceSquaredToTarget < TooCloseToTargetDistanceSquared)
			ResolveTargetTooClose();
		if (DistanceSquaredToTarget > TooFarFromTargetDistanceSquared)
			ResolveTargetTooFar();
	}

	private void ResolveTargetTooFar()
	{
		float tooFar = DistanceToTarget - _tooFarFromTargetDistance;
		Vector2 notRandomizedWayPoint = DirectionToTarget * tooFar + Unit.Position;
			
		Vector2 newWayPoint = GetRandomizedWayPoint(notRandomizedWayPoint);
		GoTo(newWayPoint);
	}

	private void ResolveTargetTooClose()
	{
		float tooClose = _tooCloseToTargetDistance - DistanceToTarget;
		Vector2 notRandomizedWayPoint = DirectionFromTarget * tooClose + Unit.Position;
			
		Vector2 newWayPoint = GetRandomizedWayPoint(notRandomizedWayPoint);
		GoTo(newWayPoint);
	}

	private Vector2 GetRandomizedWayPoint(Vector2 wayPoint)
		=> wayPoint.RandomVectorInRange(_wayPointRandomizationRange);
	
	private bool ShouldReload()
	{
		if (Weapon is null)
			return false; 
		return !Weapon.HasAmmo() && !Weapon.IsReloading;
	}

	private void UpdatePointingAt()
	{
		if (Weapon is null || TargetUnit is null)
			return;
		
		Weapon.PointingAt = TargetUnit.Position;
	}

	private void StopTargetUnit()
	{
		TargetUnit = null;
	}
	
	private void SetTargetUnit(Unit targetUnit)
	{
		if (TargetUnit is not null)
			TargetUnit.OnDeath -= SetTargetUnitToNull;
		if (targetUnit is not null)
			targetUnit.OnDeath += SetTargetUnitToNull;
		else
			_shootingTimer.Stop();
		
		TargetUnit = targetUnit;
	}

	private void SetTargetUnitToNull()
		=> SetTargetUnit(null);
}
