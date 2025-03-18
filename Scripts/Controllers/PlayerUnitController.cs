using Godot;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Input.IsActionJustPressed("shoot"))
		{
			Unit.Weapon?.Shoot();
		}
	}
	
	protected override Vector2 GetTargetDirection()
	{
		return Input.GetVector("left", "right", "up", "down");
	}
}
