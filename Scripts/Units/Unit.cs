using System;
using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class Unit : CharacterBody2D
{
	private Vector2 _targetDirection;
	
	[Export] private Weapon _weapon;
	
	[Export] private float _maxWalkSpeed;
	[Export] private float _maxWalkAcceleration;
	[Export] private float _maxBrakeAcceleration;

	public event Action<Weapon> OnWeaponChanged;

	private const float DirectionDeadZoneSquared = 0.01f;
	
	public Weapon Weapon
		=> _weapon;
	
	private float MaxAcceleration
		=> IsBraking ? _maxBrakeAcceleration : _maxWalkAcceleration;

	private bool IsBraking 
		=> _targetDirection.LengthSquared() < DirectionDeadZoneSquared;

	private float MaxAccelerationSquared 
		=> MaxAcceleration * MaxAcceleration;
	
	public override void _Ready()
	{
		if (_weapon is not null)
			AttachWeapon(_weapon);
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		UpdateWalkingVelocity((float)delta);
	}

	public void Die()
	{
		_weapon?.Drop();
		QueueFree();
	}
	
	public void SetTargetDirection(Vector2 targetDirection)
	{
		if (!targetDirection.IsNormalized())
			targetDirection = targetDirection.Normalized();
		
		_targetDirection = targetDirection;
	}

	public void SetPointingAt(Vector2 pointingAt)
	{
		_weapon.PointingAt = pointingAt;
	}

	public void DropWeapon()
		=> AttachWeapon(null);
	
	private void UpdateWalkingVelocity(float deltaSeconds)
	{
		Vector2 targetVelocity = _targetDirection * _maxWalkSpeed;
		Vector2 targetDelta = targetVelocity - Velocity;
		
		Vector2 deltaVelocity = targetDelta;
		
		if (deltaVelocity.Length() > MaxAcceleration * deltaSeconds)
			deltaVelocity = deltaVelocity.Normalized() * MaxAcceleration * deltaSeconds;
		
		Velocity += deltaVelocity;
	}

	private void AttachWeapon(Weapon weapon)
	{
		if (ReferenceEquals(_weapon, weapon))
			return;
		
		_weapon?.Drop();
		
		_weapon = weapon;
		weapon?.SetUnit(this);
		
		OnWeaponChanged?.Invoke(weapon);
	}
}
