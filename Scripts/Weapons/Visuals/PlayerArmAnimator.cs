using Godot;

namespace ULTRAmiami.Weapons.Visuals;

public partial class PlayerArmAnimator : AnimationPlayer
{
	[Export] private StringName _animationName; 
	
	private void StartAnimation()
	{
		Stop();
		Play(_animationName);
	}
}
