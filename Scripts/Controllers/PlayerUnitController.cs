using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	private static StringName _shoot = "shoot";
	private static StringName _dropWeapon = "drop weapon";
	private static StringName _pickUpWeapon = "pick up weapon";
	private static StringName _left = "left";
	private static StringName _right = "right";
	private static StringName _up = "up";
	private static StringName _down = "down";
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Unit is null) 
			return;
		
		UpdatePointingAt();
		
		if (Input.IsActionJustPressed(_shoot))
			Weapon?.TryStartShooting();
		else if (Input.IsActionPressed(_shoot))
			Weapon?.TryAutomaticShooting();

		if (Input.IsActionJustPressed(_dropWeapon))
			Unit.DropWeapon();
		if (Input.IsActionJustPressed(_pickUpWeapon))
		    Unit.PickUpWeapon();
	}

	private void UpdatePointingAt()
	{
		if (Weapon is null)
			return;
		
		Vector2 pointingAt = GetGlobalMousePosition();
		Weapon.PointingAt = pointingAt;
	}
	
	protected override Vector2 GetTargetDirection()
	{
		return Input.GetVector(_left, _right, _up, _down);
	}
}
