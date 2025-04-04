using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Unit is null) 
			return;
		
		UpdatePointingAt();
		
		if (Input.IsActionJustPressed("shoot"))
			Weapon?.TryStartShooting();
		else if (Input.IsActionPressed("shoot"))
			Weapon?.TryAutomaticShooting();

		if (Input.IsActionJustPressed("drop weapon"))
			Unit.DropWeapon();
		if (Input.IsActionJustPressed("pick up weapon"))
		    Unit.PickUpWeapon();
	}

	private void UpdatePointingAt()
	{
		if (Weapon is null)
			return;
		
		Vector2 pointingAt = Unit.GetGlobalMousePosition();
		Weapon.PointingAt = pointingAt;
	}
	
	protected override Vector2 GetTargetDirection()
	{
		return Input.GetVector("left", "right", "up", "down");
	}
}
