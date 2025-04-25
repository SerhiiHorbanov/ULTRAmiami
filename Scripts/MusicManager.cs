using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class MusicManager : AudioStreamPlayer
{
	[Export] private AudioEffect _onDeathEffect;

	private int BusIdx;
	
	public override void _Ready()
	{
		BusIdx = AudioServer.GetBusIndex(Bus);
		
		if (AudioServer.GetBusEffectCount(BusIdx) > 0)
			AudioServer.RemoveBusEffect(BusIdx, 0);
	}

	private void OnPlayerDeath(Hit _)
		=> OnPlayerDeath();
	private void OnPlayerDeath()
	{
		AudioServer.AddBusEffect(BusIdx, _onDeathEffect);
	}
}
