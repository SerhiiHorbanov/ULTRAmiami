using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Effects;

public partial class PhysicalParticle : Node2D
{
	[Export] private Vector2 _avgVelocity;
	[Export] private Vector2 _velocityRandomization;
	
	[Export] private float _friction;
	private Vector2 _velocity;
	
	[Export] private RayCast2D _rayCast;
	
	[Export] private float _randomizedRotationDegrees;
	[Export] private Node2D _rotatedPart;

	private bool _isStopped;

	[Signal]
	public delegate void OnStoppedEventHandler();

	private float HalfRandomizedRotationRad
		=> float.DegreesToRadians(_randomizedRotationDegrees) / 2;

	public void InitializeAsSiblingOfWithRotation(Node2D sibling, float rad)
	{
		if (sibling is null)
			return;
		
		this.MakeSiblingOf(sibling);
		
		_rotatedPart.Rotation = MyRandom.Range(-HalfRandomizedRotationRad, HalfRandomizedRotationRad) + rad;
		_velocity = _avgVelocity.RandomVectorInRange(_velocityRandomization);
		GlobalPosition = sibling.GlobalPosition;
		
		_velocity = _velocity.Rotated(rad);
	}
	
	public override void _Process(double delta)
	{
		if (_isStopped)
			return;

		float d = (float)delta;
		
		HandlePossibleCollision(d);
		ApplyFriction(d);
		Position += _velocity * d;
	}
	
	private void ApplyFriction(float delta)
	{
		float newLength = float.Max(_velocity.Length() - _friction * delta, 0);
		_velocity = _velocity.LimitLength(newLength);
	}
	
	private void HandlePossibleCollision(float delta)
	{
		_rayCast.TargetPosition = _velocity * delta;
		_rayCast.ForceRaycastUpdate();
		
		if (_rayCast.IsColliding() || _velocity.Length() < 1)
			Stop();
	}

	private void Stop()
	{
		_isStopped = true;
		_velocity = Vector2.Zero;
		EmitSignalOnStopped();
	}
}
