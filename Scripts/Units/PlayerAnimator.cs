using Godot;

namespace ULTRAmiami.Units;

public partial class PlayerAnimator : UnitAnimator
{
	private float _bodyRotation;
	
	protected override float CalculateBodyRotation()
	{
		return _bodyRotation;
	}

	private void UpdateBodyRotation(Vector2 relativeAimPos)
	{
		_bodyRotation = relativeAimPos.Angle();
	}
}