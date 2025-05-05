using Godot;
using ULTRAmiami.Units;
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

	private bool _isBufferingShot;

	public override void _Ready()
	{
		base._Ready();
		Unit.OnWeaponChanged += _ => _isBufferingShot = false;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Unit is null) 
			return;
		
		UpdatePointingAt();
		CheckAndResolveShooting();

		if (Input.IsActionJustPressed(_dropWeapon))
			Unit.DropWeapon();
		if (Input.IsActionJustPressed(_pickUpWeapon))
		    Unit.PickUpWeapon();
	}
	
	private void CheckAndResolveShooting()
	{
		if (_isBufferingShot && Weapon.EnoughTimeSinceLastShotToShootAgain())
		{
			Weapon.TryStartShooting();
			_isBufferingShot = false;
		}
		else if (Input.IsActionJustPressed(_shoot))
		{
			TryStartShootingAndProcessBuffering();
		}
		else if (Input.IsActionPressed(_shoot))
		{
			Weapon?.TryAutomaticShooting();
		}
	}
	private void TryStartShootingAndProcessBuffering()
	{
		bool enoughTimeSinceLastShot = Weapon?.EnoughTimeSinceLastShotToShootAgain() ?? false;
			
		if (enoughTimeSinceLastShot)
		{
			Weapon.TryStartShooting();
			return;
		}
			
		bool hasAmmo = Weapon?.HasAmmo() ?? false;
			
		_isBufferingShot |= hasAmmo;
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
