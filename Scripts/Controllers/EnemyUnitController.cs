using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class EnemyUnitController : AIUnitController
{
	private Unit _targetUnit;

	[Export] private float _tooCloseToTargetDistance;
	[Export] private float _targetDistance;
	[Export] private float _tooFarFromTargetDistance;
	[Export] private Vector2 _destinationRandomizationRange;
	
	[Export] private Timer _shootingTimer;
	[Export] private EnemyLineOfSightToTarget _lineOfSightToTarget;

	[Export] private float _targetMoveThresholdForPathRebuilding;
	[Export] private Timer _pathRebuildingSuppressionTimer;
	
	private bool _targetWasInView;
	private Weapon _weaponToUnsubscribeFrom;
	
	private float TooCloseToTargetDistanceSquared
		=> _tooCloseToTargetDistance * _tooCloseToTargetDistance;
	private float TooFarFromTargetDistanceSquared
		=> _tooFarFromTargetDistance * _tooFarFromTargetDistance;

	private float TargetMoveThresholdForPathRebuildingSquared
		=> _targetMoveThresholdForPathRebuilding * _targetMoveThresholdForPathRebuilding;
	
	private float DistanceSquaredToTarget
		=> Unit.GlobalPosition.DistanceSquaredTo(_targetUnit.GlobalPosition);
	
	private float DistanceToTarget
		=> Unit.GlobalPosition.DistanceTo(_targetUnit.GlobalPosition);
	
	private Vector2 CurrentDestinationDirection
		=> (_targetUnit.GlobalPosition - Unit.GlobalPosition).Normalized();
	
	private Vector2 DirectionFromTarget
		=> -CurrentDestinationDirection;
	
	public override void _Ready()
	{
		base._Ready();
		
		if (_targetUnit is not null)
			_targetUnit.OnDeath += DetachTargetUnit;

		PlayerNoticed += SetTargetUnit;
		PlayerNoticed += _ => _shootingTimer.Start();
		
		_shootingTimer.Timeout += TryShooting;
	}
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (_targetUnit is null || Unit is null)
			return;

		if (ShouldReload())
			ReloadWeapon();

		SetNewDestinationIfShould();
		
		UpdatePointingAt();
	}

	public override void _ExitTree()
	{
		DetachTargetUnit();
	}

	private void TryShooting()
	{
		if (_lineOfSightToTarget.IsTargetInView)
			Weapon?.TryStartShooting();
	}
	
	private void ReloadWeapon()
	{
		Weapon.Reload();
		_weaponToUnsubscribeFrom = Weapon;
		Weapon.OnReloadFinished += OnReloadFinished;
		Unit.OnDeath += UnsubscribeFromWeaponReload;
		_shootingTimer.Stop();
	}

	private void UnsubscribeFromWeaponReload(Hit _)
		=> UnsubscribeFromWeaponReload();
	private void UnsubscribeFromWeaponReload()
	{
		_weaponToUnsubscribeFrom.OnReloadFinished -= OnReloadFinished;
	}
	
	private void OnReloadFinished()
	{
		if (_targetUnit is not null)
			_shootingTimer.Start();
		Weapon.OnReloadFinished -= OnReloadFinished;
		Unit.OnDeath -= UnsubscribeFromWeaponReload;
	}

	private void SetNewDestinationIfShould()
	{
		if (!_lineOfSightToTarget.TargetWasInView)
			return;
		
		if (_pathRebuildingSuppressionTimer.TimeLeft > 0)
			return;
		
		if (!_lineOfSightToTarget.IsTargetInView)
			GoToTargetUnit();
		else if (DistanceSquaredToTarget < TooCloseToTargetDistanceSquared || DistanceSquaredToTarget > TooFarFromTargetDistanceSquared)
			GoToDistanceFromTarget(_targetDistance);
	}
	private void GoToTargetUnit()
	{
		if (!(Destination.DistanceSquaredTo(_targetUnit.GlobalPosition) > TargetMoveThresholdForPathRebuildingSquared))
			return;
		
		GoTo(_targetUnit.GlobalPosition);
		_pathRebuildingSuppressionTimer.Start();
	}

	private void GoToDistanceFromTarget(float distance)
	{
		Vector2 notRandomizedDestination = _targetUnit.GlobalPosition + DirectionFromTarget * distance;
		Vector2 newDestination = GetRandomizedDestination(notRandomizedDestination);
		
		GoTo(newDestination);
		_pathRebuildingSuppressionTimer.Start();
	}

	private Vector2 GetRandomizedDestination(Vector2 destination)
		=> destination.RandomVectorInRange(_destinationRandomizationRange);
	
	private bool ShouldReload()
	{
		if (Weapon is null)
			return false; 
		return !Weapon.HasAmmo() && !Weapon.IsReloading;
	}

	private void UpdatePointingAt()
	{
		if (Weapon is null || _targetUnit is null)
			return;
		
		if (_lineOfSightToTarget.IsTargetInView)
		{
			Weapon.PointingAt = _targetUnit.GlobalPosition;
			return;
		}
		Weapon.PointingAt = GlobalPosition + GetTargetDirection() * 100;
	}
	
	private void SetTargetUnit(Unit targetUnit)
	{
		if (_targetUnit is not null)
			_targetUnit.OnDeath -= DetachTargetUnit;
		if (targetUnit is not null)
			targetUnit.OnDeath += DetachTargetUnit;
		else
			_shootingTimer.Stop();
		
		_lineOfSightToTarget.TargetUnit = targetUnit;
		_targetUnit = targetUnit;
	}

	private void DetachTargetUnit()
		=> SetTargetUnit(null);
	private void DetachTargetUnit(Hit _)
		=> DetachTargetUnit();
}
