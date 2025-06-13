using Godot;

namespace ULTRAmiami.Units;

public partial class PlayerAnimator : UnitAnimator
{
	private PlayerAim _aim;

	private float BodyRotation; 
	
	protected override float CalculateBodyRotation()
	{
		return BodyRotation;
	}

	private void UpdateBodyRotation(Vector2 relativeAimPos)
	{
		BodyRotation = relativeAimPos.Angle();
	}
}