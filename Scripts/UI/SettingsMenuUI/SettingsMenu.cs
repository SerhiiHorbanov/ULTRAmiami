using Godot;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class SettingsMenu : Control
{
	[Export] private bool _isOpen;

	public override void _Ready()
	{
		SetOpen(_isOpen);
	}

	public void Open()
		=> SetOpen(true);
	private void Close()
		=> SetOpen(false);
	
	private void SetOpen(bool isOpen)
	{
		_isOpen = isOpen;
		Visible = isOpen;
	}
}
