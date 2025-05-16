using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.Level.Requirements;

public partial class KillCountRequirement : CompletionRequirement
{
	[Export] private int _minKills;
	[Export] private int _maxKills = UnlimitedMaxKills;
	
	private const int UnlimitedMaxKills = -1;
	
	private uint Kills
		=> PlayerScore.Current.Kills;

	public override bool IsCompleted()
		=> Kills >= _minKills && (_maxKills == UnlimitedMaxKills || Kills <= _maxKills);

	public override bool IsFailed()
		=> _maxKills != UnlimitedMaxKills && Kills > _maxKills;
}
