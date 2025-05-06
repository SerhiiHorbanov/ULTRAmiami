using Godot;
using ULTRAmiami.Effects;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class Bullet : Node2D
{
	[Export] private PackedScene _hitParticles;
	[Export] private PackedScene _bloodSplatterParticles;
	
	[Export] private float _speed;
	[Export] private float _speedRandomness;
	[Export] private RayCast2D _rayCast;
	[Export] private float _maxLifeSpan;
	private float _lifeSpan;
	
	private Vector2 _velocity;
	[Export] private float _damage;

	public override void _Ready()
	{
		_speed *= MyRandom.Range(1 - _speedRandomness, 1 + _speedRandomness);
	}

	public void Initialize(Vector2 globalPosition, Vector2 shootingAt)
	{
		GlobalPosition = globalPosition;

		Vector2 shootingDirection = (shootingAt - globalPosition).Normalized();
		_velocity = shootingDirection * _speed;
		Rotation = shootingDirection.Angle();
		UpdateRayCast((float)GetPhysicsProcessDeltaTime());
		_rayCast.ForceRaycastUpdate();
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
		
		UpdateLifeSpan((float)delta);
		UpdateRayCast((float)delta);
		
		if (_rayCast.IsColliding())
			OnHit(_rayCast.GetCollider());
	}

	private void UpdateLifeSpan(float delta)
	{
		_lifeSpan += delta;
		
		if (_lifeSpan > _maxLifeSpan)
			QueueFree();
	}

	private void UpdateRayCast(float delta)
	{
		_rayCast.TargetPosition = new(_velocity.Length() * delta, 0);
	}

	private void OnHit(GodotObject objectHit)
	{
		QueueFree();

		if (objectHit is not Node hitNode)
			return;
		
		Unit hitUnit = hitNode.GetAncestor<Unit>();

		if (hitUnit is null)
		{
			CreateParticlesOnCollisionPoint(_hitParticles);
			return;
		}
		
		HitUnit(hitUnit);
	}
	
	private void HitUnit(Unit unit)
	{
		CreateParticlesOnCollisionPoint(_bloodSplatterParticles);
		
		unit.Hit(new(_velocity, _damage));
	}

	private void CreateParticlesOnCollisionPoint(PackedScene particlesScene)
	{
		MyParticles particles = particlesScene.Instantiate<MyParticles>();
		
		particles.MakeSiblingOf(this);
		particles.GlobalRotation = GlobalRotation;
		particles.GlobalPosition = _rayCast.GetCollisionPoint();
		particles.Emit();
	}
}
