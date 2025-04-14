using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class Bullet : Node2D
{
	[Export] private PackedScene _hitParticle;
	
	[Export] private float _speed;
	[Export] private RayCast2D _rayCast;
	[Export] private float _maxLifeSpan;
	private float _lifeSpan;
	
	private Vector2 _velocity;
	
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
			CreateHitParticles();
			return;
		}
		
		hitUnit.Die();
	}
	
	private void CreateHitParticles()
	{
		CpuParticles2D particles = _hitParticle.Instantiate<CpuParticles2D>();
		
		particles.Rotation = Rotation;
		particles.MakeSiblingOf(this);
		particles.GlobalPosition = GlobalPosition;
		particles.Restart();
	}
}
