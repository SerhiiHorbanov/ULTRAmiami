using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;

namespace ULTRAmiami.UI;

public enum RestartReason
{
	PlayerDeath,
	Restart,
}

public partial class GameplayRestartConsole : ColorRect
{
	[Export] private RichTextLabel _consoleText;

	[Export(PropertyHint.File, "*.txt,")] private StringName _playerDeathRestartTextPath;
	private static string _playerDeathRestartText;
	
	[Signal]
	public delegate void OnAnimationStartedEventHandler();
	
	public override void _Ready()
	{
		ReadRestartTexts();
	}

	public void BeginDeathAnimation(Hit _)
		=> BeginDeathAnimation();
	
	public void BeginDeathAnimation()
	{
		EmitSignalOnAnimationStarted();
		_consoleText.Text = _playerDeathRestartText;
	}

	private void PrintScore()
	{
		_consoleText.Text = "";
		_consoleText.Text += $"[font_size=36][color=red][b]Blood[/b][color=white] lost: {PlayerScore.Current.BloodLost}\n";
		_consoleText.Text += $"[color=red][b]Blood[/b][color=white] consumed: {PlayerScore.Current.BloodConsumed}\n";
		_consoleText.Text += $"Enemies killed: {PlayerScore.Current.Kills}\n";
		_consoleText.Text += $"Time survived: {PlayerScore.Current.TimeAlive}\n";
		_consoleText.Text += $"Press [R] to [color=red]E N T E R T A I N  M E  A G A I N\n   ";
	}

	private void ReadRestartTexts()
	{
		FileAccess file = FileAccess.Open(_playerDeathRestartTextPath, FileAccess.ModeFlags.Read);
		_playerDeathRestartText = file.GetAsText();
		file.Close();
	}
}
