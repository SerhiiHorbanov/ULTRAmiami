using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class Unit : Node
{
	private Vector2 _targetDirection;
	
	[Export] private Weapon _weapon;
	
	private const float MaxDirectionLengthSquared = 1.01f;
	private const float DirectionDeadZoneSquared = 0.01f;
	
	[Export] private float _maxWalkSpeed;
	[Export] private float _maxWalkAcceleration;
	[Export] private float _maxBrakeAcceleration;

	[Export] private CharacterBody2D _characterBody;
	
	private Vector2 Velocity
	{
		get => _characterBody.Velocity;
		set => _characterBody.Velocity = value;
	}

	public Weapon Weapon
		=> _weapon;

	public Vector2 Position
		=> _characterBody.Position;
	
	private float MaxAcceleration
		=> _targetDirection.LengthSquared() < DirectionDeadZoneSquared ? _maxBrakeAcceleration : _maxWalkAcceleration;
	
	private float MaxAccelerationSquared 
		=> MaxAcceleration * MaxAcceleration;
	
	public override void _Ready()
	{
		CheckForWeaponInChildrenAndAttach();
	}

	private void CheckForWeaponInChildrenAndAttach()
	{
		Weapon weapon = this.GetChild<Weapon>();
		
		if (weapon is null)
			return;
		
		AttachWeapon(weapon);
	}

	public override void _PhysicsProcess(double delta)
	{
		_characterBody.MoveAndSlide();
		UpdateWalkingVelocity((float)delta);
		
	}

	private void UpdateWalkingVelocity(float deltaSeconds)
	{
		Vector2 targetVelocity = _targetDirection * MaxAcceleration;
		Vector2 targetDelta = targetVelocity - Velocity;
		
		Vector2 deltaVelocity = targetDelta;
		
		if (targetDelta.LengthSquared() > MaxAccelerationSquared * deltaSeconds)
			deltaVelocity = deltaVelocity.Normalized() * MaxAcceleration * deltaSeconds;
		
		Velocity += deltaVelocity;
	}

	public void SetTargetDirection(Vector2 targetDirection)
	{
		float lengthSquared = targetDirection.LengthSquared();
		
		if (lengthSquared > MaxDirectionLengthSquared)
			targetDirection = targetDirection.Normalized();
		
		_targetDirection = targetDirection;
	}

	public void SetPoitingAt(Vector2 pointingAt)
	{
		_weapon.PointingAt = pointingAt;
	}

	public void AttachWeapon(Weapon weapon)
	{
		_weapon = weapon;
		weapon.Reparent(this);
		weapon.Unit = this;
	}

	public void Die()
	{
		QueueFree();
	}
}
