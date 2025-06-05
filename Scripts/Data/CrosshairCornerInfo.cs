using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class CrosshairCornerInfo : Resource
{
	[Export] public Texture2D Texture { get; private set; }
	[Export] public Color Color { get; private set; } = Colors.White;
	
	[Export] public Vector2 Direction { get; private set; }
	[Export] public Vector2 Scale { get; private set; }
	[Export] public float RotationDeg { get; private set; }
	
	public float RotationRad
		=> float.DegreesToRadians(RotationDeg);
}
