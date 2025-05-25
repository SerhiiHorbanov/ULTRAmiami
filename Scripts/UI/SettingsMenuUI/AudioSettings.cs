using Godot;
using Godot.Collections;

namespace ULTRAmiami.UI.SettingsMenuUI;

[GlobalClass]
public partial class AudioSettings : Control
{
	[Export] private Array<HSlider> _sliders = [];
	[Export] private Array<VolumeSetting> _volumeSettings = [];

	public override void _Ready()
	{
		LoadAndApply();
	}

	private void SetAndApplyVolume(float volume, int settingIndex)
	{
		_volumeSettings[settingIndex].Volume = volume;
		_volumeSettings[settingIndex]?.Apply();
	}
	
	private void LoadAndApply()
	{
		for (int i = 0; i < _volumeSettings.Count; i++)
		{
			VolumeSetting setting = _volumeSettings[i];
			setting.Load();
			setting.Apply();
			_sliders[i].Value = setting.Volume;
		}
	}
	
	private void Save()
	{
		foreach (VolumeSetting setting in _volumeSettings)
			setting.Save();
	}
}
