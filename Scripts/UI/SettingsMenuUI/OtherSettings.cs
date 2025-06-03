using Godot;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class OtherSettings : Control
{
	public void SetMaxFps(int fps)
	{
		Engine.MaxFps = fps;
	}
}
