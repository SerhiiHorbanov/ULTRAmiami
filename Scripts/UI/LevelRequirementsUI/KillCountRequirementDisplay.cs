using Godot;

namespace ULTRAmiami.UI.LevelRequirementsUI;

public partial class KillCountRequirementDisplay : Control
{
	[Export] private RichTextLabel _label;
	[Export] private string _prefix = "";

	private int _min;
	private int _max;
	
	public void Initialize(int minKills, int maxKills)
	{
		_min = minKills;
		_max = maxKills;
	}
	
	public void UpdateText(uint killsAmount)
	{
		string firstNumberColor = KillsReachedMin(killsAmount) ? "[color=green]" : "[color=yellow]";
		string secondNumberColor;
		
		if (IsUnlimitedMaxKills(_max))
			secondNumberColor = $"[color=green]{_min}";
		else
			secondNumberColor = KillsReachedMin(killsAmount) ? $"[color=red]{_max + 1}" : $"[color=green]{_min}[color=white]/[color=red]{_max + 1}";

		string firstNumber = killsAmount.ToString();
		
		_label.Text = _prefix + firstNumberColor + firstNumber + "[color=white]/" + secondNumberColor;
	}
	
	private bool KillsReachedMin(uint killsAmount)
		=> killsAmount >= _min;

	private static bool IsUnlimitedMaxKills(int maxKills)
		=> maxKills == -1;
}
