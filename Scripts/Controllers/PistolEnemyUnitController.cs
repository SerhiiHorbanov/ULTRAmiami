using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PistolEnemyUnitController : AIUnitController
{
	[Export] public Unit TargetUnit;

	[Export] private float _tooCloseToTargetDistance;
	[Export] private float _tooFarFromTargetDistance;
	[Export] private Vector2 _wayPointRandomizationRange;
	
	private float TooCloseToTargetDistanceSquared
		=> _tooCloseToTargetDistance * _tooCloseToTargetDistance;
	private float TooFarFromTargetDistanceSquared
		=> _tooFarFromTargetDistance * _tooFarFromTargetDistance;
	
	private Vector2 PositionOfTarget
		=> TargetUnit.Position;

	private float DistanceSquaredToTarget
		=> Unit.Position.DistanceSquaredTo(PositionOfTarget);
	
	private float DistanceToTarget
		=> Unit.Position.DistanceTo(PositionOfTarget);
	
	private Vector2 DirectionToTarget
		=> (PositionOfTarget - Unit.Position).Normalized();
	
	private Vector2 DirectionFromTarget
		=> -DirectionToTarget;
	
	public override void _Ready()
	{
		base._Ready();
		
		if (TargetUnit is not null)
			TargetUnit.OnDeath += StopTargetUnit;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (TargetUnit is null || Unit is null)
			return;

		if (ShouldReload())
			Weapon.Reload();

		SetNewWayPointIfShould();
		
		UpdatePointingAt();
		Weapon.TryStartShooting();
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
		Vector2 notRandomizedWayPoint = DirectionToTarget * tooFar;
			
		Vector2 newWayPoint = GetRandomizedWayPoint(notRandomizedWayPoint);
		GoTo(newWayPoint);
	}

	private void ResolveTargetTooClose()
	{
		float tooClose = _tooCloseToTargetDistance - DistanceToTarget;
		Vector2 notRandomizedWayPoint = DirectionFromTarget * tooClose;
			
		Vector2 newWayPoint = GetRandomizedWayPoint(notRandomizedWayPoint);
		GoTo(newWayPoint);
	}

	private Vector2 GetRandomizedWayPoint(Vector2 wayPoint)
		=> wayPoint.RandomVectorInRange(_wayPointRandomizationRange);
	
	private bool ShouldReload()
		=> !Weapon.HasAmmo() && !Weapon.IsReloading;

	private void UpdatePointingAt()
	{
		if (Weapon is null || TargetUnit is null)
			return;
		
		Weapon.PointingAt = PositionOfTarget;
	}

	private void StopTargetUnit()
	{
		TargetUnit = null;
	}
}
