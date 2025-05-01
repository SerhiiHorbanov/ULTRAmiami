using Godot;

namespace ULTRAmiami.UI;

public partial class FpsCounter : Control
{
	[Export] private Label _text;
	
	private StringName _fpsText = "FPS: ";
	public override void _Process(double delta)
	{
		_text.Text = _fpsText + Engine.GetFramesPerSecond();
	}
}
