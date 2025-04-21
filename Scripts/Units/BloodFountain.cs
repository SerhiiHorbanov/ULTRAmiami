using System.Collections.Generic;
using Godot;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Units;

public partial class BloodFountain : Node2D
{
	[Export] private float _bloodAmount;
	[Export] private Timer _timer;
	
	[Export] private Curve _fountainAreaSizeCurve;
	[Export] private Area2D _fountainArea;

	private float BloodPerSecond
		=> _bloodAmount / FountainTime;
	private float FountainTime
		=> (float)_timer.WaitTime;
	
	private readonly Dictionary<Node2D, BloodFuel> _bloodSuppliedTo = [];
	
	public override void _Process(double delta)
	{
		float size = _fountainAreaSizeCurve.Sample((float)_timer.TimeLeft / FountainTime);
		_fountainArea.Scale = new(size, size);
		
		foreach (BloodFuel bloodFuel in _bloodSuppliedTo.Values)
		{
			bloodFuel.AddBlood(BloodPerSecond * (float)delta);
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not Unit)
			return;

		BloodFuel bloodFuel = body.GetChild<BloodFuel>();
		
		if (bloodFuel is null)
			return;
		
		_bloodSuppliedTo.Add(body, bloodFuel);
	}

	private void OnBodyExited(Node2D body)
	{
		_bloodSuppliedTo.Remove(body);
	}

	private void OnFountainFinished()
	{
		QueueFree();
	}
}
