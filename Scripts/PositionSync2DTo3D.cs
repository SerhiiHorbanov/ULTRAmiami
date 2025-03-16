using Godot;

namespace ULTRAmiami;

public partial class PositionSync2DTo3D : Node
{
	[Export] private Node2D _syncFrom;
	[Export] private Node3D _syncTo;
	
	public override void _Process(double d)
	{
		_syncTo.Position = GetPosition3D();
	}

	private Vector3 GetPosition3D()
	{
		Vector2 position2D = _syncFrom.Position;
		Vector3 position3D = new(position2D.X, 0, position2D.Y);
		
		return position3D;
	}
}
