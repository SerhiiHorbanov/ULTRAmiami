using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.Units;

public partial class Hit(Vector2 force, float damage, HitBleedingInfo bleedingInfo) : GodotObject
{
	public Vector2 Force = force;
	public float Damage = damage;
	
	public  HitBleedingInfo BleedingInfo = bleedingInfo;
}
