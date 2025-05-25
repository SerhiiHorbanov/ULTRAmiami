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
		Node node = InstantiateUI();

		if (node is not KillCountRequirementDisplay ui)
		{
			GD.PrintErr("Instantiated node is not a KillCountRequirementDisplay");
			return null;
		}
		
		ui.Initialize(_minKills, _maxKills);
		OnKillsChanged += ui.UpdateText;
		
		EmitSignalOnKillsChanged(Kills);
		
		return node;
	}

	public override bool IsCompleted()
		=> Kills >= _minKills && (_maxKills == UnlimitedMaxKills || Kills <= _maxKills);

	public override bool IsFailed()
		=> _maxKills != UnlimitedMaxKills && Kills > _maxKills;
}
