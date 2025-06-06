namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class DynamicCameraSetting : SettingToggle
{
	public override void _Ready()
	{
		OnValueChanged += UnitFollowingCamera.SetDynamic;
		base._Ready();
	}
}
