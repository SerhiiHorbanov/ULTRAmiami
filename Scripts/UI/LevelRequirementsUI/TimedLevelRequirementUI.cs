using Godot;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class TimedLevelRequirementUI : Control
{
	[Export] private string _prefix;
	[Export] private RichTextLabel _timeLeftLabel;

	private float _lastTimeUpdatedRounded;
	private Timer _timer;
	
	public void Initialize(Timer timer)
	{
		_timer = timer;
	}

	public override void _Process(double delta)
	{
		UpdateTime((float)_timer.TimeLeft);
	}

	public void UpdateTime(float time)
	{
		time = float.Round(time, 2);
		
		if (time == _lastTimeUpdatedRounded)
			return;
		
		_lastTimeUpdatedRounded = time;
		
		_timeLeftLabel.Text = _prefix + time.ToString("0.00");
	}
}
