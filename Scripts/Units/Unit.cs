using System;
using System.Collections.Generic;
using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class Unit : CharacterBody2D
{
	private Vector2 _targetDirection;
	
	[Export] private Weapon _weapon;
	[Export] private bool _dropsPickUppableWeapon;
	
	[Export] private bool _godMode;
	
	[ExportGroup("Movement")]
	[Export] private float _maxWalkAcceleration;
	[Export] private float _maxBrakeAcceleration;
	[Export] private float _maxWalkSpeed;

	[ExportSubgroup("Redirection")]
	[Export] private float _maxRedirectionSpeed;
	[Export] private float _redirectionAcceleration;
	private bool _isRedirectioning;
	private bool _isRecoveringFromRedirection;

	public readonly List<DroppedWeapon> EnteredDroppedWeapons = [];

	public event Action<Weapon> OnWeaponChanged;
	public event Action OnDeath;

	private const float DirectionDeadZoneSquared = 0.01f;
	
	public Weapon Weapon
		=> _weapon;

	private bool IsBraking 
		=> _targetDirection.LengthSquared() < DirectionDeadZoneSquared;
	
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
		if (_godMode)
			return;
		
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
		DroppedWeapon closestDroppedWeapon = EnteredDroppedWeapons[0];
		float closestDistanceSquared = Position.DistanceSquaredTo(EnteredDroppedWeapons[0].Position);
		
		foreach (DroppedWeapon weapon in EnteredDroppedWeapons)
		{
			float distanceSquared = Position.DistanceSquaredTo(weapon.Position);
			if (distanceSquared < closestDistanceSquared)
			{
				closestDroppedWeapon = weapon;
				closestDistanceSquared = distanceSquared;
			}
		}

		return closestDroppedWeapon.Weapon;
	}

	private void UpdateWalkingVelocity(float deltaSeconds)
	{
		Vector2 targetVelocity = _targetDirection * GetMaxSpeed();
		Vector2 targetDelta = targetVelocity - Velocity;
		
		UpdateIsRedirectioning(targetDelta);

		float maxAcceleration = GetMaxAcceleration();
		Vector2 deltaVelocity = targetDelta.LimitLength(maxAcceleration * deltaSeconds);
		
		Velocity += deltaVelocity;
	}

	private float GetMaxSpeed()
		=> _isRedirectioning ? _maxRedirectionSpeed : _maxWalkSpeed;

	private void UpdateIsRedirectioning(Vector2 targetDelta)
	{
		if (_dropsPickUppableWeapon)
			return;
		
		if (_isRecoveringFromRedirection)
			_isRecoveringFromRedirection = targetDelta.LengthSquared() > 50f;
		
		if (_isRedirectioning && !_isRecoveringFromRedirection)
		{
			_isRedirectioning = targetDelta.LengthSquared() > 0.1f;
			if (!_isRedirectioning)
				_isRecoveringFromRedirection = true;
		}
		else if (!IsBraking && !_isRecoveringFromRedirection)
		{
			_isRedirectioning = targetDelta.CosineSimilarity(Velocity) < -0.7f;
		}
	}
	
	private float GetMaxAcceleration()
	{
		if (IsBraking)
			return _maxBrakeAcceleration;
		if (_isRedirectioning)
			return _redirectionAcceleration;
		return _maxWalkAcceleration;
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
