namespace ULTRAmiami;

public struct PlayerScore
{
	public uint Kills { get; private set; }
	public float BloodConsumed { get; private set; }
	public float BloodLost { get; private set; }

	public static PlayerScore Current;
	
	public void AddKill()
		=> Kills++;
	
	public void AddBloodConsumed(float value)
		=> BloodConsumed += value;
	
	public void AddBloodLost(float value)
		=> BloodLost += value;
}
