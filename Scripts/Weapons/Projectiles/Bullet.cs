using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class Bullet : Node2D
{
	[Export] private float _speed;
	[Export] private RayCast2D _rayCast;
	[Export] private float _maxLifeSpan;
	private float _lifeSpan;
	
	private Vector2 _velocity;
	
	public void Initialize(Vector2 position, Vector2 shootingAt)
	{
		Position = position;

		Vector2 shootingDirection = (shootingAt - position).Normalized();
		_velocity = shootingDirection * _speed;
		Rotation = shootingDirection.Angle();
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
		
		hitUnit?.Die();
	}
}
