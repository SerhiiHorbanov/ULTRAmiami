using Godot;
using ULTRAmiami.UI.LevelRequirementsUI;
using Timer = Godot.Timer;

namespace ULTRAmiami.Requirements;

public partial class TimeLimitationRequirement : CompletionRequirement
{
	[Export] private Timer _timer;
	private bool _timeIsOut;

	public override void _Ready()
	{
		base._Ready();
		
		_timer.Timeout += Timeout;
		_timer.Start();
	}

	public override Node InstantiateAndConnectToUI()
	{
		Node node = InstantiateUI();

		if (node is not TimedLevelRequirementUI ui)
		{
			GD.PrintErr("Instantiated node is not a TimedLevelRequirementUI");
			return null;
		}
		
		ui.Initialize(_timer);
		return ui;
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
