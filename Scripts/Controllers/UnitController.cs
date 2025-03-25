using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public abstract partial class UnitController : Node2D
{
	[Export] private Unit _unit;

	protected Unit Unit
	{
		get => _unit;
		set
		{
			if (_unit is not null)
				_unit.OnDeath -= DetachUnit;
			
			if (value is not null)
				value.OnDeath += DetachUnit;
			
			_unit = value;
		}
	}
	
	protected Weapon Weapon
		=> Unit.Weapon;

	protected bool HasUnit 
		=> Unit is not null;
	
	public override void _Ready()
	{
		Unit.OnDeath += DetachUnit;
	}

	public override void _ExitTree()
	{
		Unit.OnDeath -= DetachUnit;
	}

	private void DetachUnit()
	{
		Unit = null;
	}
	
	public override void _Process(double delta)
	{
		if (Unit == null)
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
