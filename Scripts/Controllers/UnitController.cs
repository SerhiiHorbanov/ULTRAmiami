using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public abstract partial class UnitController : Node2D
{
	[Export] private Unit _unit;

	protected Unit Unit
		=> _unit;

	protected Weapon Weapon
		=> Unit.Weapon;
	
	public override void _Ready()
	{
		if (Unit is null)
			return;
		
		Unit.OnDeath += DetachUnit;
	}

	public override void _ExitTree()
	{
		if (Unit is null)
			return;
		
		Unit.OnDeath -= DetachUnit;
		SetUnit(null);
	}

	private void SetUnit(Unit newUnit)
	{
		if (_unit is not null)
			_unit.OnDeath -= DetachUnit;
			
		if (newUnit is not null)
			newUnit.OnDeath += DetachUnit;
			
		_unit?.SetTargetDirection(Vector2.Zero);
		
		_unit = newUnit;
	}
	
	private void DetachUnit(Hit _)
		=> SetUnit(null);
	
	public override void _Process(double delta)
	{
		if (Unit is null)
			return;
		
		UpdateTargetDirection();
	}
	
	private void UpdateTargetDirection()
	{
		Vector2 targetDirection = GetTargetDirection();
		Unit?.SetTargetDirection(targetDirection);
	}
	
	protected abstract Vector2 GetTargetDirection();
}
