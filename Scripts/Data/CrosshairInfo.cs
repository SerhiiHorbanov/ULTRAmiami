using Godot;
using Godot.Collections;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class CrosshairInfo : Resource
{
	[Export] public Texture2D CenterTexture { get; private set; }
	[Export] public Vector2 CenterScale { get; private set; } = new(5, 5);
	
	[Export] public float ShownSpreadDeg { get; private set; }
	[Export] public Array<CrosshairCornerInfo> Corners { get; private set; }
	
	public float HalfShownSpreadRad
		=> float.DegreesToRadians(ShownSpreadDeg) * 0.5f;
}
