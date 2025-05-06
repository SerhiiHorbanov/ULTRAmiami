using Godot;

namespace ULTRAmiami.Units;

public partial class PlayerArmAnimator : Sprite2D
{
	[Export] private float _restingRotationDeg;
	[Export] private float _speedDeg;
	[Export] private float _endRotationDeg;
	private bool _isPlaying;

	private float RestingRotationRad
		=> float.DegreesToRadians(_restingRotationDeg);
	private float SpeedRad
		=> float.DegreesToRadians(_speedDeg);
	private float EndRotationRad
		=> float.DegreesToRadians(_endRotationDeg);
	
	public override void _Process(double delta)
	{
		if (_isPlaying)
			Rotate(SpeedRad * (float)delta);
		
		if (Rotation < EndRotationRad)
			return;

		_isPlaying = false;
		Rotation = RestingRotationRad;
	}
	
	private void StartAnimation()
		=> _isPlaying = true;
}