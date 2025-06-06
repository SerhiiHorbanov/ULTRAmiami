using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class BreakableTrigger : Node2D, IAttackable
{
	private bool _isBroken;
	
	[Export] private Node2D _notBrokenGraphics;
	[Export] private Node2D _brokenGraphics;

	public bool Bleeds
		=> false;
	
	[Signal]
	public delegate void OnBrokenEventHandler();

	public override void _Ready()
	{
		RemoveChild(_brokenGraphics);
	}

	public void Hit(Hit hit)
	{
		if (_isBroken)
			return;
		
		_isBroken = true;
		_notBrokenGraphics.QueueFree();
		AddChild(_brokenGraphics);
		EmitSignalOnBroken();
	}
}