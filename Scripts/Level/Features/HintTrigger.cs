using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;

namespace ULTRAmiami.Level.Features;

public partial class HintTrigger : Area2D
{
	[Export] private GameplayHint _hint;
	
	private void OnBodyEntered(Node2D body)
	{
		if (body is not Unit unit)
			return;
		if (!unit.IsPlayer)
			return;
		
		_hint.Display();
		QueueFree();
	}
}
