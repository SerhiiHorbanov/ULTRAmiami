using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class DeathCamera : Camera2D
{
	[Export] private float _rotationSpeedDeg;
	[Export] private float _speed;
	[Export] private float _zoomSpeed;

	[Export] private Timer _curveTimer;
	[Export] private Curve _speedCurve;
	[Export] private Curve _rotationCurve;
	[Export] private Curve _zoomCurve;

	private float _currentZoomSpeed;
	
	private float _deltaRotationRad;
	
	private Vector2 _direction;
	private Vector2 _velocity;

	public void Initialize(Vector2 globalPosition, Hit killingHit)
	{
		GlobalPosition = globalPosition;
		_direction = killingHit.Force.Normalized();
	}

	private void UpdateValuesByCurves()
	{
		float t = 1 - (float)(_curveTimer.TimeLeft / _curveTimer.WaitTime);
		_velocity = _direction * _speed * _speedCurve.Sample(t);
		_currentZoomSpeed = _zoomSpeed * _zoomCurve.Sample(t);
		
		float rotationSpeedRad = float.DegreesToRadians(_rotationSpeedDeg);
		_deltaRotationRad = rotationSpeedRad * _rotationCurve.Sample(t);
		if (_direction.X < 0)
			_deltaRotationRad = -_deltaRotationRad;
	}
	
	public override void _Process(double delta)
	{
		float d = (float)delta;
		
		UpdateValuesByCurves();
		
		Zoom += Vector2.One * _currentZoomSpeed * d;
		Position += _velocity * d;
		Rotation += _deltaRotationRad * d;
	}
}
