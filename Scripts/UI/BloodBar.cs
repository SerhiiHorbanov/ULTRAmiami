using Godot;

namespace ULTRAmiami.UI;

public partial class BloodBar : Control
{
	[Export] private Control _fill;
	[Export] private Control _fill2;
	[Export] private float _min;
	[Export] private float _max;
	private float _maxHeight;
	private float _maxHeight2;

	public override void _Ready()
	{
		_maxHeight = _fill.Size.Y;
		_maxHeight2 = _fill2.Size.Y;
	}

	private void SetBlood(float blood)
	{
		float height = GetHeight(blood, _maxHeight);
		float height2 = GetHeight(blood, _maxHeight2);
		
		height = float.Clamp(height, 0, _maxHeight);
		height2 = float.Clamp(height2, 0, _maxHeight2);
		
		_fill.Size = new(_fill.Size.X, height);
		_fill2.Size = new(_fill2.Size.X, height2);
	}

	private float GetHeight(float blood, float maxHeight)
		=> maxHeight * (blood - _min) / (_max - _min);
}
