using Godot;
using Godot.Collections;

namespace ULTRAmiami.UI.SettingsMenuUI;

[GlobalClass]
public partial class AudioSettings : Control
{
	public void ApplyVolume(float volume, int busIdx)
	{
		AudioServer.SetBusVolumeLinear(busIdx, volume);
	}
}
