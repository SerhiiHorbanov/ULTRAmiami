using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class AttackableAsGodotObject(IAttackable attackable) : GodotObject
{
	public void Hit(Hit hit)
		=> Attackable.Hit(hit);
	public IAttackable Attackable = attackable;
}
