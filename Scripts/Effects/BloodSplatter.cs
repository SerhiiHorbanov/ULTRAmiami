using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Effects;

public partial class BloodSplatter : Node2D
{
	[Export] private float _scaleRandomization;
	[Export] private float _animationSpeedScaleRandomization;
	
	[Export] private AnimatedSprite2D _animation;

	[Export] private float _bloodColorRandomization;
	[Export] private Timer _bloodDryingTimer;
	[Export] private Color _initialBloodColor;
	[Export] private Color _dryBloodColor;
	[Export] private Node2D _sprite;
	
	public void Initialize(Unit bleeder, Hit hit)
	{
		this.MakeSiblingOf(bleeder);

		HitBleedingInfo bleedingInfo = hit.BleedingInfo;
		
		float randomRotationOffset = MyRandom.Range(bleedingInfo.HalfRotationRandomizationRad);
		GlobalRotation = hit.Force.Angle() + randomRotationOffset;
		
		float randomizedDistanceToBleeder = bleedingInfo.DistanceToBleeder + MyRandom.Range(bleedingInfo.DistanceToBleederRandomization);
		Vector2 offsetFromBleeder = Vector2.FromAngle(GlobalRotation) * randomizedDistanceToBleeder;
		GlobalPosition = bleeder.GlobalPosition + offsetFromBleeder;
		
		Scale *= 1 + MyRandom.Range(_scaleRandomization);
		_animation.SpeedScale *= 1 + MyRandom.Range(_animationSpeedScaleRandomization);
		
		RandomizeColor();
	}
	private void RandomizeColor()
	{
		float r = MyRandom.Range(1 - _bloodColorRandomization, 1);
		
		float a = _initialBloodColor.A;
		_initialBloodColor *= r;
		_initialBloodColor.A = a;
		
		a = _dryBloodColor.A;
		_dryBloodColor *= r;
		_dryBloodColor.A = a;
		
		Modulate = _initialBloodColor;
	}

	//public override void _Process(double delta)
	//{
	//	if (_bloodDryingTimer.IsStopped())
	//		return;
	//
	//	float t = 1 - (float)(_bloodDryingTimer.TimeLeft / _bloodDryingTimer.WaitTime);
	//	Modulate = _initialBloodColor.Lerp(_dryBloodColor, t);
	//}

	private void Simplify()
	{
		_sprite.MakeSiblingOf(this);
		_sprite.Modulate = Modulate;
		QueueFree();
	}
}
