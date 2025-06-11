using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class BreakableTrigger : Node2D, IAttackable
{
	private bool _isBroken;

	public bool Bleeds
		=> false;
	
	[Signal]
	public delegate void OnBrokenEventHandler();

	public void Hit(Hit hit)
	{
		if (_isBroken)
			return;
		
		_isBroken = true;
		EmitSignalOnBroken();
	}
}