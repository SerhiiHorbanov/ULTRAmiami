using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Effects;

public partial class BloodSplatter : Node2D
{
	[Export] private float _distanceToBleeder;
	[Export] private float _distanceToBleederRandomization;
	
	[Export] private float _rotationRandomization;
	[Export] private float _scaleRandomization;
	[Export] private float _animationSpeedScaleRandomization;

	[Export] private AnimatedSprite2D _animation;
	
	private float HalfRotationRandomizationRad
		=> float.DegreesToRadians(_rotationRandomization * 0.5f);
	
	public void Initialize(Unit bleeder, Hit hit)
	{
		this.MakeSiblingOf(bleeder);

		float randomizedDistanceToBleeder = _distanceToBleeder + MyRandom.Range(_distanceToBleederRandomization);
		Vector2 offsetFromBleeder = hit.Force.Normalized() * randomizedDistanceToBleeder;
		GlobalPosition = bleeder.GlobalPosition + offsetFromBleeder;
		
		float randomRotationOffset = MyRandom.Range(HalfRotationRandomizationRad);
		GlobalRotation = hit.Force.Angle() + randomRotationOffset;
		
		Scale *= 1 + MyRandom.Range(_scaleRandomization);
		_animation.SpeedScale *= 1 + MyRandom.Range(_animationSpeedScaleRandomization);
	}
}
