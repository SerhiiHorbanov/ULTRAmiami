using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Controllers;

public abstract partial class UnitController : Node2D
{
	[Export] protected Unit Unit;
	
	public override void _Process(double delta)
	{
		UpdateTargetDirection();
	}
	
	protected void UpdateTargetDirection()
	{
		Vector2 targetDirection = GetTargetDirection();
		Unit.SetTargetDirection(targetDirection);
	}
	
	protected abstract Vector2 GetTargetDirection();
}
