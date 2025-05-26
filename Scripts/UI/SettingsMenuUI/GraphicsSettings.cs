using Godot;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class GraphicsSettings : Control 
{
	[Export] private WorldEnvironment _worldEnvironment;
	private Environment environment;

	private void EnsureEnvironment()
	{
		if (environment is not null)
			return;
		environment = _worldEnvironment.GetEnvironment();
	}
	
	private void SetBrightness(float value)
	{
		EnsureEnvironment();
		environment.AdjustmentBrightness = value;
	}

	private void SetContrast(float value)
	{
		EnsureEnvironment();
		environment.AdjustmentContrast = value;
	}

	private void SetSaturation(float value)
	{
		EnsureEnvironment();
		environment.AdjustmentSaturation = value;
	}
}
