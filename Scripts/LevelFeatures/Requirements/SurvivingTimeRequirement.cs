using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Requirements;

public partial class SurvivingTimeRequirement : CompletionRequirement
{
	private bool _isDead;
	private bool _timeIsOut;
	private bool _isCompleted;
	
	[Export] private Timer _timer;
	[Export] private Unit _player;
	
	public override void _Ready()
	{
		base._Ready();
		
		_timer.Timeout += Timeout;
		_player.OnDeath += OnDeath;
		_timer.Start();
	}

	private void OnDeath(Hit hit)
	{
		_isDead = true;
		_player.OnDeath -= OnDeath;
	}

	private void Timeout()
	{
		_timeIsOut = true;
		_isCompleted = !_isDead;
	}
	
	public override bool IsCompleted()
		=> _isCompleted;

	public override bool IsFailed()
		=> _isDead && !_timeIsOut && !_isCompleted;
}
