using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class SettingToggle : Control
{
	[Export(PropertyHint.None, "Must be a a legal file name")] private string _settingName;
	[Export] private SettingToggleData _data;
	[Export(PropertyHint.Dir)] private string _path;
	[Export] private Button _toggle;

	[Signal]
	public delegate void OnValueChangedEventHandler(bool value);
	
	public override void _Ready()
	{
		_data.Load(GetSaveFilePath());
		_toggle.ButtonPressed = _data.Value;
		EmitSignalOnValueChanged(_data.Value);
	}
	
	private string GetSaveFilePath()
		=> _path + _settingName + ".tres";
	
	private void Save()
	{
		_data.Save(_path, _settingName + ".tres");
	}
	
	private void SetValue(bool value)
	{
		_data.Value = value;
		EmitSignalOnValueChanged(value);
		Save();
	}
}
