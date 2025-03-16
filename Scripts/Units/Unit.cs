using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class Unit : Node
{
	private Vector2 _targetDirection;
	
	private const float MaxDirectionLengthSquared = 1.01f;
	[Export] private float _maxWalkSpeed;
	[Export] private float _walkAcceleration;

	[Export] private CharacterBody2D _characterBody;
	
	private Vector2 Velocity
	{
		get => _characterBody.Velocity;
		set => _characterBody.Velocity = value;
	}
	
	private float WalkAccelerationSquared 
		=> _walkAcceleration * _walkAcceleration;
	
	public override void _PhysicsProcess(double delta)
	{
		_characterBody.MoveAndSlide();
		UpdateWalkingVelocity((float)delta);

		GD.Print("Velocity = " + Velocity);
		GD.Print("Speed = " + Velocity.Length());
	}

	private void UpdateWalkingVelocity(float deltaSeconds)
	{
		Vector2 targetVelocity = _targetDirection * _maxWalkSpeed;
		Vector2 targetDelta = targetVelocity - Velocity;
		
		Vector2 deltaVelocity = targetDelta;
		
		if (targetDelta.LengthSquared() > WalkAccelerationSquared * deltaSeconds)
			deltaVelocity = deltaVelocity.Normalized() * _walkAcceleration * deltaSeconds;
		
		Velocity += deltaVelocity;
	}

	public void SetTargetDirection(Vector2 targetDirection)
	{
		float lengthSquared = targetDirection.LengthSquared();
		
		if (lengthSquared > MaxDirectionLengthSquared)
			targetDirection = targetDirection.Normalized();
		
		_targetDirection = targetDirection;
	}

	public void Die()
	{ }
}
