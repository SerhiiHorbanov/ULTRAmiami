using Godot;
using Timer = Godot.Timer;

namespace ULTRAmiami.Level.Requirements;

public partial class TimeLimitationRequirement : CompletionRequirement
{
	[Export] private Timer _timer;
	private bool _timeIsOut;

	public override void _Ready()
	{
		_timer.Timeout += Timeout;
		_timer.Start();
	}

	private void Timeout()
	{
		_timeIsOut = true;
	}

	public override bool IsCompleted()
		=> !_timeIsOut;

	public override bool IsFailed()
		=> _timeIsOut;
}