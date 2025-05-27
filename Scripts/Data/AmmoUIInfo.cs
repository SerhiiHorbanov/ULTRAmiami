using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class AmmoUIInfo : Resource
{
	[Export] public Texture2D Texture { get; private set; }
	
	[Export] public Vector2 StartingPosition { get; private set; }
	[Export] public float StartingRotation { get; private set; }
	
	[Export] public Vector2 DeltaPosition { get; private set; }
	[Export] public float DeltaRotation { get; private set; }
}
