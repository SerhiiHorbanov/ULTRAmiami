using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public interface IAttackable
{
	public bool Bleeds { get; }
	
	public void Hit(Hit hit);
}
