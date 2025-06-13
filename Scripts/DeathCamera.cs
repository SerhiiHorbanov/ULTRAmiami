using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class DeathCamera : Camera2D
{
	[Export] private float _rotationSpeedDeg;
	[Export] private float _speed;
	[Export] private float _zoomSpeed;
	
	private float _deltaRotationRad;
	private Vector2 _velocity;

	public void Initialize(Vector2 globalPosition, Hit killingHit)
	{
		GlobalPosition = globalPosition;
		_velocity = killingHit.Force.Normalized() * _speed;
		
		float rotationSpeedRad = float.DegreesToRadians(_rotationSpeedDeg);
		_deltaRotationRad = killingHit.Force.X > 0 ? rotationSpeedRad : -rotationSpeedRad;
	}
	
	public override void _Process(double delta)
	{
		float d = (float)delta;
		
		Zoom += Vector2.One * _zoomSpeed * d;
		Position += _velocity * d;
		Rotation += _deltaRotationRad * d;
	}
}
