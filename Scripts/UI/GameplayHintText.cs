using System.Collections.Generic;
using Godot;
using ULTRAmiami.Data;

namespace ULTRAmiami.UI;

public partial class GameplayHintText : Control
{
	private readonly Queue<GameplayHint> _hintsQueue = [];
	
	[Export] private Timer _timer;
	[Export] private RichTextLabel _label;
	
	[Export] private AnimationPlayer _animationPlayer;

	private bool _isHidden;
	[Export] private StringName _openAnimation;
	[Export] private StringName _closeAnimation;
	
	private static GameplayHintText _instance;
	
	public static void AddHintToQueue(GameplayHint hint)
	{
		_instance.AddHintToQueueInternal(hint);
	}
	
	public override void _Ready()
	{
		_isHidden = true;
		Visible = false;
		
		_instance = this;
	}

	public override void _ExitTree()
	{
		_instance = null;
	}

	private void AddHintToQueueInternal(GameplayHint hint)
	{
		_hintsQueue.Enqueue(hint);
		
		if (_hintsQueue.Count == 1)
			NextHint();
	}

	private void OnHintTimeElapsed()
	{
		if (_hintsQueue.Count == 0)
			HideText();
		else
			NextHint();
	}

	private void NextHint()
	{
		if (_isHidden)
			ShowText();
		
		GameplayHint next = _hintsQueue.Dequeue();
		OverrideCurrentHint(next);
	}

	private void OverrideCurrentHint(GameplayHint hint)
	{
		if (hint is null)
			HideText();
		
		_timer.WaitTime = hint?.Time ?? 0;
		_timer.Start();
		_label.Text = hint?.Text;
	}
	
	private void ShowText()
	{
		_animationPlayer.Play(_openAnimation);
		_isHidden = false;
	}
	
	private void HideText()
	{
		_animationPlayer.Play(_closeAnimation);
		_isHidden = true;
	}
}
