using Godot;

namespace ULTRAmiami.UI;

public partial class BloodBar : Control
{
	[Export] private Color _goingUpColor;
	
	[Export] private Gradient _defaultColor;
	[Export] private float _bloodDifferenceForGradientEndColor;
	
	[Export] private ColorRect _fill;
	[Export] private ColorRect _fill2;
	[Export] private float _min;
	[Export] private float _max;
	private float _maxHeight;
	private float _maxHeight2;

	private float _previousBlood;

	[Export] private int _deltaSamplesAmount = 60;
	private float[] _bloodDeltaSamples;
	private int _oldestSampleIndex;

	public override void _Ready()
	{
		_bloodDeltaSamples = new float[_deltaSamplesAmount];
		
		_maxHeight = _fill.Size.Y;
		_maxHeight2 = _fill2.Size.Y;
	}

	private void SetBlood(float blood)
	{
		if (_previousBlood == 0)
			_previousBlood = blood;
		
		UpdateBloodDeltaSamples(blood);
		
		Color color = DecideCurrentColor(blood);
		
		_fill.Color = color;
		_fill2.Color = color;
		
		UpdateHeightForFill(_fill, blood, _maxHeight);
		UpdateHeightForFill(_fill2, blood, _maxHeight2);
		
		_previousBlood = blood;
	}
	
	private void UpdateBloodDeltaSamples(float blood)
	{
		_bloodDeltaSamples[_oldestSampleIndex] = blood - _previousBlood;
		_oldestSampleIndex++;
		_oldestSampleIndex %= _deltaSamplesAmount;
	}

	private void UpdateHeightForFill(ColorRect fill, float blood, float maxHeight)
	{
		float height = GetHeight(blood, maxHeight);
		fill.Size = new(_fill.Size.X, height);
	}

	private float CalculateAverageBloodDelta()
	{
		float sum = 0;

		foreach (float delta in _bloodDeltaSamples)
			sum += delta;
		
		return sum / _deltaSamplesAmount;
	}
	
	private Color DecideCurrentColor(float blood)
	{
		float averageDelta = CalculateAverageBloodDelta();
		
		if (averageDelta > 0)
			return _goingUpColor;
		
		float t = -averageDelta / _bloodDifferenceForGradientEndColor;
		return _defaultColor.Sample(t);
	}

	private float GetHeight(float blood, float maxHeight)
	{
		float height = maxHeight * (blood - _min) / (_max - _min);
		return float.Clamp(height, 0, maxHeight);
	}
}
