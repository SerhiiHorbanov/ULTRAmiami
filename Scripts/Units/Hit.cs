using Godot;

namespace ULTRAmiami.Units;

public partial class Hit(Vector2 force, float damage, int splatterAmount = 1) : GodotObject
{
	public Vector2 Force = force;
	public float Damage = damage;
	public int SplatterAmount = splatterAmount;
}
