using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.UI;

public partial class GradientOverTimeApplier : Node
{
	[Export] private ColorRect _colorRect;
	[Export] private Gradient _gradient;
	[Export] private Timer _time;
	
	private bool _finished;

	public override void _Ready()
	{
		_time.Timeout += () => _finished = true;
	}

	public void Start()
		=> _time.Start();

	public override void _Process(double delta)
	{
		if (_finished) 
			return;
		
		float t = _time.GetTimePercentage();
		Color color = _gradient.Sample(t);
		_colorRect.Color = color;
	}
}
