using Godot;

namespace ULTRAmiami.UI;

public partial class BloodBar : Control
{
	[Export] private Control _fill;
	[Export] private float _max;
	private float _maxHeight;

	public override void _Ready()
	{
		_maxHeight = _fill.Size.Y;
	}

	private void SetBlood(float blood)
	{
		float height = _maxHeight * blood / _max;
		_fill.Size = new(_fill.Size.X, height);
	}
}
