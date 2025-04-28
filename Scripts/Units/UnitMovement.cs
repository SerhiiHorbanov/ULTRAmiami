using System;
using System.Collections.Generic;
using Godot;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class UnitMovement : Node
{
	[Export] private Unit _unit;
	
	private Vector2 _targetDirection;
	
	[ExportGroup("Movement")]
	[Export] private float _maxWalkAcceleration;
	[Export] private float _maxBrakeAcceleration;
	[Export] private float _maxWalkSpeed;

	[ExportSubgroup("Redirection")]
	[Export] private float _maxRedirectionSpeed;
	[Export] private float _redirectionAcceleration;
	private bool _isRedirectioning;
	private bool _isRecoveringFromRedirection;
	
	public Vector2 Velocity 
	{ 
		get => _unit.Velocity; 
		set => _unit.Velocity = value; 
	}
	
	private bool IsBraking 
		=> _targetDirection.LengthSquared() < DirectionDeadZoneSquared;
	
	private const float DirectionDeadZoneSquared = 0.01f;
	
	public void SetTargetDirection(Vector2 targetDirection)
	{
		if (!targetDirection.IsNormalized())
			targetDirection = targetDirection.Normalized();
		
		_targetDirection = targetDirection;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		UpdateWalkingVelocity((float)delta);
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
}
