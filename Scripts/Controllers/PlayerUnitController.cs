using Godot;

namespace ULTRAmiami.Controllers;

public partial class PlayerUnitController : UnitController
{
	protected override Vector2 GetTargetDirection()
	{
		return Input.GetVector("left", "right", "up", "down");
	}
}
