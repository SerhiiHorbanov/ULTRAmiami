using System;
using System.Collections.Generic;
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
	[Export] private bool _dropsPickUppableWeapon;

	public readonly List<Weapon> EnteredDroppedWeapons = [];

	public event Action<Weapon> OnWeaponChanged;
	public event Action OnDeath;

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
		{
			Weapon weaponToAttach = _weapon;
			_weapon = null;
			TryAttachWeapon(weaponToAttach);
		}
		
		OnDeath += DropWeapon;
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
		UpdateWalkingVelocity((float)delta);
	}

	public void Die()
	{
		OnDeath?.Invoke();
		DropWeapon();
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
		=> TryAttachWeapon(null);

	public void PickUpWeapon()
	{
		if (EnteredDroppedWeapons.Count == 0)
			return;
		
		Weapon closestWeapon = GetClosestEnteredDroppedWeapon();

		TryAttachWeapon(closestWeapon);
	}

	private Weapon GetClosestEnteredDroppedWeapon()
	{
		Weapon closestWeapon = EnteredDroppedWeapons[0];
		float closestDistanceSquared = Position.DistanceSquaredTo(EnteredDroppedWeapons[0].Position);
		
		foreach (Weapon weapon in EnteredDroppedWeapons)
		{
			float distanceSquared = Position.DistanceSquaredTo(weapon.Position);
			if (distanceSquared < closestDistanceSquared)
			{
				closestWeapon = weapon;
				closestDistanceSquared = distanceSquared;
			}
		}

		return closestWeapon;
	}

	private void UpdateWalkingVelocity(float deltaSeconds)
	{
		Vector2 targetVelocity = _targetDirection * _maxWalkSpeed;
		Vector2 targetDelta = targetVelocity - Velocity;
		
		Vector2 deltaVelocity = targetDelta;
		
		if (deltaVelocity.Length() > MaxAcceleration * deltaSeconds)
			deltaVelocity = deltaVelocity.Normalized() * MaxAcceleration * deltaSeconds;
		
		Velocity += deltaVelocity;
	}

	private void TryAttachWeapon(Weapon weapon)
	{
		if (ReferenceEquals(_weapon, weapon))
			return;

		Weapon oldWeapon = _weapon;
		_weapon = weapon;
		
		OnWeaponChanged?.Invoke(weapon);
		
		oldWeapon?.TryAttachUnit(null, _dropsPickUppableWeapon);
		weapon?.TryAttachUnit(this);
	}
}
