namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class BloodSplatterSlider : SettingSlider
{
	public static float BloodSplatterMultiplier { get; private set; } = 1;
	
	private void SetBloodSplatterMultiplier(float value)
		=> BloodSplatterMultiplier = value;
}
