using Godot;
using Godot.Collections;

namespace ULTRAmiami.UI.SettingsMenuUI;

public partial class SettingsMenu : Control
{
	[Export] private bool _isOpen;

	[Export] private Array<Control> _tabs = [];
	
	public override void _Ready()
	{
		SetOpen(_isOpen);
	}

	public void Open()
		=> SetOpen(true);

	public void Close()
		=> SetOpen(false);
	
	private void SetOpen(bool isOpen)
	{
		_isOpen = isOpen;
		Visible = isOpen;
	}

	private void SetTab(int index)
	{
		foreach (Control tab in _tabs)
			tab.Visible = false;
		
		_tabs[index].Visible = true;
	}
}
