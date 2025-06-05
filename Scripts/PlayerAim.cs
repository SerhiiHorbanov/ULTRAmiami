using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami;

public partial class PlayerAim : Node2D
{
	[Export] private Camera2D _camera;
	
	[Signal]
	public delegate void OnPositionUpdatedEventHandler(Vector2 pos);

	public override void _Ready()
	{
		_camera ??= GetTree().GetCurrentScene().GetChild<Camera2D>();
	}

	public override void _Process(double delta)
	{
		Vector2 viewOffset = _camera.GetViewportRect().Size * 0.5f;
		Vector2 mouse = _camera.GetViewport().GetMousePosition();
		
		Vector2 relativePos = mouse - viewOffset;
		EmitSignalOnPositionUpdated(relativePos);
	}
}
