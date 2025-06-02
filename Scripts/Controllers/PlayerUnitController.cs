using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	[Export] private MeleeAttacker _meleeAttacker;
	
	private static StringName _shoot = "shoot";
	private static StringName _meleeAttack = "attack melee";
	private static StringName _dropWeapon = "drop weapon";
	private static StringName _pickUpWeapon = "pick up weapon";
	private static StringName _left = "left";
	private static StringName _right = "right";
	private static StringName _up = "up";
	private static StringName _down = "down";

	private bool _isBufferingShot;

	private bool _isHoldingLeft;
	private bool _isHoldingRight;
	private bool _isHoldingUp;
	private bool _isHoldingDown;

	private bool _isJustPressingShoot;
	private bool _isHoldingShoot;
	
	public override void _Ready()
	{
		base._Ready();
		Unit.OnWeaponChanged += _ => _isBufferingShot = false;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
			return;
		
		HandleInputEvent(@event);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
			HandleInputEvent(@event);
	}

	private void HandleInputEvent(InputEvent @event)
	{
		ResolveEventForMovement(@event);
		CheckAndResolveShooting(@event);
		
		if (@event.IsActionPressed(_dropWeapon))
			Unit.DropWeapon();
		if (@event.IsActionPressed(_pickUpWeapon))
			Unit.PickUpWeapon();
		if (@event.IsActionPressed(_meleeAttack))
			_meleeAttacker.TryAttack();
	}
	
	private void ResolveEventForMovement(InputEvent @event)
	{
		if (_isHoldingLeft)
			_isHoldingLeft &= !@event.IsActionReleased(_left);
		else
			_isHoldingLeft |= @event.IsActionPressed(_left);
		
		if (_isHoldingRight)
			_isHoldingRight &= !@event.IsActionReleased(_right);
		else
			_isHoldingRight |= @event.IsActionPressed(_right);
		
		if (_isHoldingUp)
			_isHoldingUp &= !@event.IsActionReleased(_up);
		else
			_isHoldingUp |= @event.IsActionPressed(_up);
		
		if (_isHoldingDown)
			_isHoldingDown &= !@event.IsActionReleased(_down);
		else
			_isHoldingDown |= @event.IsActionPressed(_down);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		if (Unit is null) 
			return;


		if (_isJustPressingShoot)
		{
			Weapon?.TryStartShooting();
			_isJustPressingShoot = false;
		}
		if (_isBufferingShot)
			TryStartShootingAndProcessBuffering();
		else if (_isHoldingShoot)
			Weapon?.TryAutomaticShooting();
		
		UpdatePointingAt();
	}
	
	private void CheckAndResolveShooting(InputEvent @event)
	{
		if (@event.IsActionPressed(_shoot))
		{
			_isHoldingShoot = true;
			_isJustPressingShoot = true;
		}
		
		_isHoldingShoot &= !@event.IsActionReleased(_shoot);
	}
	
	private void TryStartShootingAndProcessBuffering()
	{
		if (Weapon is null)
		{
			_isBufferingShot = false;
			return;
		}
		
		bool shootingBufferedShot = Weapon?.EnoughTimeSinceLastShotToShootAgain() ?? false;

		if (!shootingBufferedShot)
			return;
		
		Weapon?.TryStartShooting();
		_isBufferingShot = false;
	}

	private void UpdatePointingAt()
	{
		_meleeAttacker?.SetRotation(GetLocalMousePosition().Angle());

		if (Weapon is null)
			return;
		
		Vector2 pointingAt = GetGlobalMousePosition();
		Weapon.PointingAt = pointingAt;
	}
	
	protected override Vector2 GetTargetDirection()
	{
		Vector2 direction = Vector2.Zero;
		
		if (_isHoldingLeft)
			direction += Vector2.Left;
		else if (_isHoldingRight)
			direction += Vector2.Right;
		if (_isHoldingUp)
			direction += Vector2.Up;
		else if (_isHoldingDown)
			direction += Vector2.Down;
		
		return direction;
	}
}
