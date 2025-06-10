using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class SpecificUnitKillRequirementUI : Node
{
	[Export] private RichTextLabel _label;
	
	[Export] private string _notCompletedPrefix;
	[Export] private string _completedPrefix;
	[Export] private string _completedSuffix;
	private string _targetName;

	public void Initialize(string targetName)
	{
		_targetName = targetName;
		_label.Text = _notCompletedPrefix + targetName;
	}
	
	public void ChangeTextToCompletedVersion()
	{
		_label.Text = _completedPrefix + _targetName + _completedSuffix;
	}
}
