using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class InfModeScore : Resource
{
	[Export] public PlayerScore Score;

	public InfModeScore()
	{ }
	public InfModeScore(PlayerScore score)
	{
		Score = score;
	}
	

	public float Time
		=> Score.TimeAlive;
	public int Kills
		=> (int)Score.Kills;
	
	public static int CompareByTime(InfModeScore a, InfModeScore b)
		=> a.Score.TimeAlive.CompareTo(b.Score.TimeAlive);
	public static int CompareByKills(InfModeScore a, InfModeScore b)
		=> a.Score.Kills.CompareTo(b.Score.Kills);
	public static int CompareByBloodConsumed(InfModeScore a, InfModeScore b)
		=> a.Score.BloodConsumed.CompareTo(b.Score.BloodConsumed);
	public static int CompareByBloodLost(InfModeScore a, InfModeScore b)
		=> a.Score.BloodConsumed.CompareTo(b.Score.BloodConsumed);
}
