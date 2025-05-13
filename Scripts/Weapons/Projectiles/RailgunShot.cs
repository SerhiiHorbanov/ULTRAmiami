using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class RailgunShot : Node2D
{
	[Export] private RayCast2D _rayCast;
	[Export] private float _length;
	[Export] private float _damage;
	//[Export] private AnimationPlayer _animationPlayer;
	private Vector2 _direction;
	
	public void Initialize(Vector2 globalPosition, Vector2 shootingAt)
	{
		GlobalPosition = globalPosition;
		_direction = shootingAt - globalPosition;
		SetTargetPosition(_direction * _length);
		
		_rayCast.ForceRaycastUpdate();
		while (_rayCast.IsColliding())
		{
			GodotObject collider = _rayCast.GetCollider();
			TryAttackUnitByCollider(collider);
			_rayCast.AddException((CollisionObject2D)collider);
			_rayCast.ForceRaycastUpdate();
		}
		QueueFree();
	}

	private void SetTargetPosition(Vector2 targetPosition)
	{
		_rayCast.TargetPosition = targetPosition;
		
	}
	
	private void TryAttackUnitByCollider(GodotObject target)
	{
		if (target is not Node2D node2D)
			return;
		
		Unit unit = node2D.GetAncestor<Unit>();
		
		if (unit is null)
			return;
		
		unit.Hit(new(_direction, _damage));
	}
}
