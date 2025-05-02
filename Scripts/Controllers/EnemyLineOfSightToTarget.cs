using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Controllers;

public partial class EnemyLineOfSightToTarget : RayCast2D
{
	public Unit TargetUnit;

	public bool IsTargetInView { get; private set; }
	public bool TargetWasInView { get; private set; }
	
	public override void _PhysicsProcess(double delta)
	{
		if (TargetUnit is null)
			return;
		
		TargetPosition = TargetUnit.GlobalPosition - GlobalPosition;
		
		UpdateIsTargetInView();
	}

	private void UpdateIsTargetInView()
	{
		IsTargetInView = false;
		
		if (!IsColliding())
			return;
		
		GodotObject godotObject = GetCollider();

		if (godotObject is not Node2D hitNode)
			return;
		
		HitUnit(hitNode.GetAncestor<Unit>());
	}
	
	private void HitUnit(Unit unit)
	{
		if (unit is null)
			return;
		
		bool isTarget = unit == TargetUnit;
		IsTargetInView = isTarget;
		TargetWasInView |= isTarget;
		if (isTarget)
			return;
		
		AddException(unit);
		ForceRaycastUpdate();
		UpdateIsTargetInView();
	}
}
