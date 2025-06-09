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
	}
}
