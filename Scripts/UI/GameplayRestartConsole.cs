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
	[Export] private TextOverTimePrinter _consoleText;

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
		_consoleText.ClearAndPrint(_playerDeathRestartText);
		_consoleText.OnFinished += PrintScore;
	}

	private void PrintScore()
	{
		_consoleText.OnFinished -= PrintScore;
		_consoleText.ClearText();
		_consoleText.Print($"[font_size=36][color=red][b]Blood[/b][color=white] lost: {PlayerScore.Current.BloodLost}\n");
		_consoleText.Print($"[color=red][b]Blood[/b][color=white] consumed: {PlayerScore.Current.BloodConsumed}\n");
		_consoleText.Print($"Enemies killed: {PlayerScore.Current.Kills}\n");
		_consoleText.Print($"Press [R] to [color=red]E N T E R T A I N  M E  A G A I N\n   ");
	}

	private void ReadRestartTexts()
	{
		FileAccess file = FileAccess.Open(_playerDeathRestartTextPath, FileAccess.ModeFlags.Read);
		_playerDeathRestartText = file.GetAsText();
		file.Close();
	}
}
