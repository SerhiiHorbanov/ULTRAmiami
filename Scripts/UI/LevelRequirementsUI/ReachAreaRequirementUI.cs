using Godot;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class ReachAreaRequirementUI : Control
{
	[Export] private RichTextLabel _label;
	[Export] private string _prefixBeforeReached;
	
	[Export] private string _prefixAfterReached;
	[Export] private string _suffixAfterReached;
	
	private string _areasName;

	public void Initialize(string areasName)
	{
		_areasName = areasName;
		
		_label.Text = _prefixBeforeReached + _areasName;
	}

	public void OnAreaReached()
	{
		_label.Text = _prefixAfterReached + _areasName + _suffixAfterReached;
	}
}
