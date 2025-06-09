using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class HitBleedingInfo : Resource
{
	[Export] public int SplatterAmount;
	
	[Export] public float DistanceToBleeder;
	[Export] public float DistanceToBleederRandomization;
	
	[Export] private float _rotationRandomizationDeg;
	
	public float HalfRotationRandomizationRad
		=> float.DegreesToRadians(_rotationRandomizationDeg * 0.5f);
}
