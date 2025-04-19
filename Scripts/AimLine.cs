using Godot;
using System;
using ULTRAmiami.Units;

public partial class AimLine : Line2D
{
	[Export] private Unit _unit;
	[Export] private RayCast2D _rayCast;
	[Export] private float _maxLength;
	
	private Vector2 PointingAt
		=> _unit.Weapon.PointingAt;

	private Vector2 PointingDirection
		=> (PointingAt - GlobalPosition).Normalized();
	
	public override void _Process(double delta)
	{
		if (!ShouldBeShown())
		{
			RemovePoint(1);	
			return;
		}

		UpdateLine();
	}
	
	private void UpdateLine()
	{
		Vector2 direction = PointingDirection;
		UpdateRaycast(direction);

		if (_rayCast.IsColliding())
			SetLineEnd(_rayCast.GetCollisionPoint() - GlobalPosition);
		else
			SetLineEnd(direction * _maxLength);
	}
	
	private void UpdateRaycast(Vector2 direction)
	{
		_rayCast.TargetPosition = direction * _maxLength;
		_rayCast.ForceRaycastUpdate();
	}
	
	private void SetLineEnd(Vector2 endPosition)
	{
		RemovePoint(1);
		AddPoint(endPosition);
	}


	private bool ShouldBeShown()
		=> _unit.Weapon is not null;
}
