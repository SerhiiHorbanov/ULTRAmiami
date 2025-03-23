using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	private Weapon Weapon
		=> Unit.Weapon;
	
	public override void _Process(double delta)
	{
		base._Process(delta);
		
		UpdatePointingAt();
		
		if (Input.IsActionJustPressed("shoot"))
			Weapon?.TryStartShooting();
		else if (Input.IsActionPressed("shoot"))
			Weapon?.TryAutomaticShooting();
	}

	private void UpdatePointingAt()
	{
		if (Weapon is null)
			return;
		
		Vector2 pointingAt = GetLocalMousePosition();
		Weapon.PointingAt = pointingAt;
	}
	
	protected override Vector2 GetTargetDirection()
	{
		return Input.GetVector("left", "right", "up", "down");
	}
}
