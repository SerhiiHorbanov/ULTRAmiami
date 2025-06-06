using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class LevelCompletionInfo : Resource
{
	[Export] public bool IsCompleted { get; private set; }

	[Export] public PlayerScore BestTimeScore { get; private set; }
	[Export] public PlayerScore MaxKillsScore { get; private set; }
	[Export] public PlayerScore MaxBloodScore { get; private set; }

	public void AddCompletion(PlayerScore score)
	{
		IsCompleted = true;
		
		if (BestTimeScore is null || score.TimeAlive < BestTimeScore?.TimeAlive)
			BestTimeScore = score;
		
		if (MaxBloodScore is null || score.BloodConsumed > MaxBloodScore.BloodConsumed)
			MaxBloodScore = score;
		
		if (MaxKillsScore is null || score.Kills >= MaxKillsScore.Kills)
		{
			if (MaxKillsScore is not null && score.Kills == MaxKillsScore.Kills && score.TimeAlive > MaxKillsScore.TimeAlive)
				return;
			MaxKillsScore = score;
		}
	}
}
