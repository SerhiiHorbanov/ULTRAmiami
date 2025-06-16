using System;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class FlamethrowerFire : RigidBody2D, IFirearmProjectile
{
	[Export] private int _framesCount;
	[Export] private AnimatedSprite2D _sprite;
	[Export] private AnimatedSprite2D _outlineSprite;
	
	[Export] private float _rotationRandomizationDeg;
	[Export] private float _velocityForRotationSettling;
	
	[Export] private float _friction;
	[Export] private float _maxShootingDistance;

	[Export] private float _randomizationMaxLength;

	private float _randomizedRotation;
	private float _rotationByVelocity;
	
	private float HalfRotationRandomizationRad
		=> float.DegreesToRadians(_rotationRandomizationDeg) * 0.5f;
	
	private Vector2 Velocity
	{
		get => GetLinearVelocity();
		set => SetLinearVelocity(value);
	}
	
	private float VelocityForRotationSettlingSquared
		=> _velocityForRotationSettling * _velocityForRotationSettling;

	public void Initialize(Vector2 globalPosition, Vector2 shootingAt)
	{
		GlobalPosition = globalPosition;
		
		Velocity = CalculateInitialVelocity(shootingAt - globalPosition);
		InitializeRotation();
		InitializeAnimationOffset();
	}

	private void InitializeAnimationOffset()
	{
		int frame = Random.Shared.Next(_framesCount);
		_sprite.Frame = frame;
		_outlineSprite.Frame = frame;
	}

	private void InitializeRotation()
	{
		_randomizedRotation = MyRandom.Range(HalfRotationRandomizationRad);
		_rotationByVelocity = float.Pi * -0.5f + Velocity.Angle();
		
		Rotation = _rotationByVelocity;
	}

	private Vector2 RandomizeShootingAt(Vector2 shootingAt)
	{
		float randomizationLength = MyRandom.Range(0, _randomizationMaxLength);
		float angle = MyRandom.Range(float.Pi * 2);
		
		Vector2 randomizer = new Vector2(randomizationLength, 0).Rotated(angle);
		
		return shootingAt + randomizer;
	}

	private Vector2 CalculateInitialVelocity(Vector2 d)
	{
		d = d.LimitLength(_maxShootingDistance);
		d = RandomizeShootingAt(d);
		
		float dl = d.Length();
		float vl = float.Sqrt(dl * _friction * 2);
		
		return d.Normalized() * vl;
	}

	private void OnBodyEntered(Node2D body)
	{
		(body as Unit)?.TrySetOnFire();
	}

	public override void _PhysicsProcess(double delta)
	{
		ApplyFriction(delta);
		UpdateRotation();
	}

	private void UpdateRotation()
	{
		float vLenSq = Velocity.LengthSquared();
		
		if (vLenSq > VelocityForRotationSettlingSquared)
			return;
		
		float vl = float.Sqrt(vLenSq);

		float t = vl / _velocityForRotationSettling;
		Rotation = float.Lerp(_rotationByVelocity, _randomizedRotation, 1 - t);
	}

	private void ResolveCollision(KinematicCollision2D collision)
	{
		if (collision is null)
			return;
		if (collision.GetCollider() is Unit)
			return;

		Vector2 normal = collision.GetNormal();
		Velocity -= normal * Velocity.Dot(normal);
	}

	private void ApplyFriction(double delta)
	{
		float vl = float.Max(Velocity.Length() - (_friction * (float)delta), 0);
		Velocity = Velocity.LimitLength(vl);
	}
}
