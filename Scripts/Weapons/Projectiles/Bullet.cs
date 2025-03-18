using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class Bullet : Node2D
{
	[Export] private float _speed;
	[Export] private RayCast2D _rayCast;
	
	private Vector2 _velocity;
	
	public void Initialize(Weapon shotFrom)
	{
		Position = shotFrom.Position;
		_velocity = shotFrom.PointingInDirection * _speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
		
		UpdateRayCast((float)delta);
		
		if (_rayCast.IsColliding())
			OnHit(_rayCast.GetCollider());
	}

	private void UpdateRayCast(float delta)
	{
		_rayCast.TargetPosition = _velocity * delta;
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
