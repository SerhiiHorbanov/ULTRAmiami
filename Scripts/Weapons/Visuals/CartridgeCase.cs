using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Visuals;

public partial class CartridgeCase : Node2D
{
	[Export] private Vector2 _avgVelocity;
	[Export] private Vector2 _velocityRandomization;
	
	[Export] private float _friction;
	private Vector2 _velocity;
	
	[Export] private RayCast2D _rayCast;
	
	[Export] private float _randomizedRotationDegrees;
	[Export] private Node2D _rotatedPart;

	private bool _isStopped;
	
	private float HalfRandomizedRotationRad
		=> float.DegreesToRadians(_randomizedRotationDegrees) / 2;

	public override void _Ready()
	{
		_rotatedPart.Rotation = MyRandom.Range(-HalfRandomizedRotationRad, HalfRandomizedRotationRad);
	}

	public void Initialize(float rotationRad)
	{
		_velocity = _avgVelocity.RandomVectorInRange(_velocityRandomization);
		_velocity = _velocity.Rotated(rotationRad);
	}
	
	public override void _Process(double delta)
	{
		if (_isStopped)
			return;
		
		_rayCast.TargetPosition = _velocity * (float)delta;
		_rayCast.ForceRaycastUpdate();
		
		if (_rayCast.IsColliding() || _velocity.Length() < 1)
			Stop();
		
		Position += _velocity * (float)delta;

		float newLength = float.Max(_velocity.Length() - _friction * (float)delta, 0);
		_velocity = _velocity.LimitLength(newLength);
	}

	private void Stop()
	{
		_velocity = Vector2.Zero;
		_isStopped = true;
	}
}
