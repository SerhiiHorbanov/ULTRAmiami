using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons.Projectiles;

public partial class RailgunShot : Node2D
{
	[Export] private ShapeCast2D _shapeCast;
	[Export] private Line2D _line;
	[Export] private float _length;
	[Export] private float _damage;
	
	private Vector2 _direction;
	
	public void Initialize(Vector2 globalPosition, Vector2 shootingAt)
	{
		GlobalPosition = globalPosition;
		_direction = (shootingAt - globalPosition).Normalized();
		SetTargetPosition(_direction * _length);
		
		ResolveAllCollisions();
	}
	private void ResolveAllCollisions()
	{
		_shapeCast.ForceShapecastUpdate();
		
		int collisionsAmount = _shapeCast.GetCollisionCount();
		while (collisionsAmount > 0)
		{
			_shapeCast.ForceShapecastUpdate();
			collisionsAmount = _shapeCast.GetCollisionCount();
			
			for (int i = 0; i < collisionsAmount; i++)
			{
				GodotObject collider = _shapeCast.GetCollider(i);
				TryAttackUnitByCollider(collider);
				_shapeCast.AddException(collider as CollisionObject2D);
				_shapeCast.AddException(null);
			}
		}
	}

	private void SetTargetPosition(Vector2 targetPosition)
	{
		_shapeCast.TargetPosition = targetPosition;
		
		_line.ClearPoints();
		_line.AddPoint(Vector2.Zero);
		_line.AddPoint(targetPosition);
	}
	
	private void TryAttackUnitByCollider(GodotObject target)
	{
		if (target is not Node2D node2D)
			return;
		
		IAttackable attackable = node2D.GetAncestor<IAttackable>();

		attackable?.Hit(new(_direction, _damage));
	}
}
