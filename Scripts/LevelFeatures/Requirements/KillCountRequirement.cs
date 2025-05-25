using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.UI.LevelRequirementsUI;

namespace ULTRAmiami.Requirements;

public partial class KillCountRequirement : CompletionRequirement
{
	[Export] private int _minKills;
	[Export] private int _maxKills = UnlimitedMaxKills;
	
	private const int UnlimitedMaxKills = -1;

	private uint _prevKills;
	
	private uint Kills
		=> PlayerScore.Current.Kills;

	[Signal]
	public delegate void OnKillsChangedEventHandler(uint newValue);
	
	public override void _Process(double delta)
	{
		if (Kills == _prevKills)
			return;
		
		EmitSignalOnKillsChanged(Kills);
		_prevKills = Kills;
	}

	public override Node InstantiateAndConnectToUI()
	{
		Node ui = InstantiateUI();

		if (ui is not KillCountRequirementDisplay killCountRequirementDisplay)
		{
			GD.PrintErr("KillCountRequirementDisplay is not a KillCountRequirementDisplay");
			return null;
		}
		
		killCountRequirementDisplay.Initialize(_minKills, _maxKills);
		OnKillsChanged += killCountRequirementDisplay.UpdateText;
		
		EmitSignalOnKillsChanged(Kills);
		
		return ui;
	}

	public override bool IsCompleted()
		=> Kills >= _minKills && (_maxKills == UnlimitedMaxKills || Kills <= _maxKills);

	public override bool IsFailed()
		=> _maxKills != UnlimitedMaxKills && Kills > _maxKills;
}
