using Godot;

namespace ULTRAmiami.Data;

public partial class PlayerScore : GodotObject
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
