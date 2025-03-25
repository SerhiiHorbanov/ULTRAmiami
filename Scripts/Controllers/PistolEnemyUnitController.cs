using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PistolEnemyUnitController : AIUnitController
{
	[Export] public Unit TargetUnit;

	private Vector2 PositionOfTarget
		=> TargetUnit.Position;

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

		if (!Weapon.HasAmmo() && !Weapon.IsReloading)
			Weapon.Reload();
		
		UpdatePointingAt();
		Weapon.TryStartShooting();
	}

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
