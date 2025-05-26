using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class SettingSlider : Control
{
	[Export(PropertyHint.None, "Must be a a legal file name")] private string _settingName;
	[Export] private SettingSliderData _data;
	[Export(PropertyHint.Dir)] private string _path;
	[Export] private HSlider _slider;

	[Signal]
	public delegate void OnValueChangedEventHandler(float value);
	
	public override void _Ready()
	{
		_data.Load(GetSaveFilePath());
		EmitSignalOnValueChanged(_data.Value);
		_slider.Value = _data.Value;	
	}

	private string GetSaveFilePath()
		=> _path + _settingName + ".tres";

	private void Save(bool valueChanged)
	{
		if (valueChanged)
			Save();
	}
	
	private void Save()
	{
		_data.Save(_path, _settingName + ".tres");
	}

	private void SetValue(float value)
	{
		_data.Value = value;
		EmitSignalOnValueChanged(value);
	}
}
