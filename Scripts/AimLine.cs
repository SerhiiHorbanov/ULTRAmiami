using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class AimLine : Line2D
{
	[Export] private Unit _unit;
	[Export] private RayCast2D _rayCast;
	[Export] private float _maxLength;

	private WeaponAimLineInfo _info;
	
	private Vector2 PointingAt
		=> _unit.Weapon.PointingAt;

	private Vector2 PointingDirection
		=> (PointingAt - GlobalPosition).Normalized();
	
	public override void _Process(double delta)
	{
		ClearPoints();

		if (!ShouldBeShown())
			return;

		_info = _unit.Weapon.AimLineInfo;
		
		DefaultColor = _unit.Weapon.HasAmmo() ? _info.Color : _info.NoAmmoColor;
		
		Width = _info.LineWidth;
		
		if (_info.ShowSpread)		
			CreateLinesWithSpread();
		else
			CreateSingleLine();
	}

	private void CreateSingleLine()
	{
		Vector2 dir = PointingDirection;

		Vector2 collision = GetCollisionByDirection(dir);
		
		AddPointFromGlobal(collision);
		AddPoint(Vector2.Zero);
	}
	
	private void CreateLinesWithSpread()
	{
		Vector2 leftDir = PointingDirection.Rotated(-_unit.Weapon.HalfSpreadRadians);
		Vector2 rightDir = PointingDirection.Rotated(_unit.Weapon.HalfSpreadRadians);
		
		AddPointFromGlobal(GetCollisionByDirection(leftDir));
		AddPoint(Vector2.Zero);
		AddPointFromGlobal(GetCollisionByDirection(rightDir));
	}

	private void AddPointFromGlobal(Vector2 point)
		=> AddPoint(point - GlobalPosition);
	
	private Vector2 GetCollisionByDirection(Vector2 dir)
	{
		if (!_info.StopOnHit)
			return dir * _maxLength + GlobalPosition;
		
		UpdateRaycast(dir);
		
		if (_rayCast.IsColliding())
			return _rayCast.GetCollisionPoint();
		return dir * _maxLength + GlobalPosition;
	}

	private void UpdateRaycast(Vector2 direction)
	{
		_rayCast.TargetPosition = direction * _maxLength;
		_rayCast.ForceRaycastUpdate();
	}

	private bool ShouldBeShown()
		=> _unit?.Weapon?.AimLineInfo is not null;
}
