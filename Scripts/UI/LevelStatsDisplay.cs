using System;
using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI;

public partial class LevelStatsDisplay : Control
{
	[Export] private Level _level;

	[Export] private string _namePrefix;
	[Export] private RichTextLabel _nameLabel;

	[Export] private RichTextLabel _currKillsLabel;
	[Export] private RichTextLabel _currTimeLabel;
	[Export] private RichTextLabel _currBloodLabel;
	
	[Export] private Label _bestKillsLabel;
	[Export] private Label _bestTimeLabel;
	[Export] private Label _bestBloodLabel;

	private LevelCompletionInfo _completionInfo;

	private int _shownKills;
	private float _shownBlood;
	
	private float _bestTime;
	private int _bestKills;
	private float _bestBlood;
	
	private const string Green = "[color=green]";
	private const string Red = "[color=red]";
	private const string Yellow = "[color=yellow]";
	
	private readonly static StringName Action = "toggle stats display";
	
	public override void _Ready()
	{
		if (_level is null)
		{
			QueueFree();
			return;
		}

		_level.OnCompletion += Hide;
		_nameLabel.Text = _namePrefix + _level.LevelName;
		
		InitializeBestStatsLabels();
	}

	private void InitializeBestStatsLabels()
	{
		_bestTime = _level.GetBestTime();
		_bestKills = _level.GetBestKills();
		_bestBlood = _level.GetBestBlood();
		
		_bestBloodLabel.Text = _bestBlood.ToString("0.00");
		_bestTimeLabel.Text = _bestTime.ToString("0.000");
		_bestKillsLabel.Text = _bestKills.ToString();
	}

	public override void _Process(double delta)
	{
		if (Visible)
			UpdateStatsLabels();
	}

	private void UpdateStatsLabels()
	{
		float currTime = PlayerScore.Current.TimeAlive;
		int currKills = (int)PlayerScore.Current.Kills;
		float currBlood = PlayerScore.Current.BloodConsumed;
		
		if (_shownKills != currKills)
		{
			string killsColor = currKills == _bestKills ? Yellow : currKills < _bestKills ? Red : Green;
			_currKillsLabel.Text = killsColor + currKills;
			_shownKills = currKills;
		}

		if (Math.Abs(_shownBlood - currBlood) > 0.01f)
		{
			string bloodColor = currBlood > _bestBlood ? Green : Red;
			_currBloodLabel.Text = bloodColor + currBlood.ToString("0.00");
			_shownBlood = currBlood;
		}

		string timeColor = currTime < _bestTime ? Green : Red;
		_currTimeLabel.Text = timeColor + currTime.ToString("0.00");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed(Action))
			Visible = !Visible;
	}
}
