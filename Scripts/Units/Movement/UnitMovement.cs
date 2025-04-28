using Godot;

namespace ULTRAmiami.Units.Movement;

public partial class UnitMovement : Node
{
	[Export] private Unit _unit;
	
	private Vector2 _targetDirection;
	
	[ExportGroup("Movement")]
	[Export] private float _maxWalkAcceleration;
	[Export] private float _maxBrakeAcceleration;
	[Export] private float _maxWalkSpeed;
	
	public Vector2 Velocity 
	{ 
		get => _unit.Velocity; 
		set => _unit.Velocity = value; 
	}
	
	protected Vector2 TargetDirection
		=> _targetDirection;
	
	protected bool IsBraking 
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

		float maxAcceleration = GetMaxAcceleration();
		Vector2 deltaVelocity = targetDelta.LimitLength(maxAcceleration * deltaSeconds);
		
		Velocity += deltaVelocity;
	}
	
	protected virtual float GetMaxSpeed()
		=> _maxWalkSpeed;

	protected virtual float GetMaxAcceleration()
		=> IsBraking ? _maxBrakeAcceleration : _maxWalkAcceleration;
}
