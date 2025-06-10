using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ULTRAmiami.Data;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Effects;

public partial class BloodSplatterSystem : Node2D
{
	private record struct SplatterInfo(Node2D Node);

	private HitBleedingInfo _bleedingInfo;
	private List<SplatterInfo> _splatterList;
	
	[Export] private float _scaleRandomization;
	[Export] private float _bloodColorRandomization;
	
	[Export] private Color _initialBloodColor;
	[Export] private Color _dryBloodColor;
	[Export] private Timer _bloodDryingTimer;

	[Export] private Timer _deletionTimer;

	[Export] private Array<PackedScene> _splatterScenes;
	
	public void AddSplattersFromHit(Unit bleeder, Hit hit)
	{
		_splatterList.Capacity = _splatterList.Count + hit.BleedingInfo.SplatterAmount;
		
		for (int i = 0; i < hit.BleedingInfo.SplatterAmount; i++)
		{
			SplatterInfo splatter = SpawnAndInitializeSingleSplatter(bleeder, hit);
			_splatterList.Add(splatter);
		}
	}
	
	public void Initialize(Unit bleeder, Hit hit)
	{
		this.MakeSiblingOf(bleeder);
		
		_bleedingInfo = hit.BleedingInfo;
		_splatterList = new();
		_splatterList.Capacity = _bleedingInfo.SplatterAmount;
		
		AddSplattersFromHit(bleeder, hit);
	}

	private SplatterInfo SpawnAndInitializeSingleSplatter(Unit bleeder, Hit hit)
	{
		Node2D splatter = _splatterScenes.PickRandom().Instantiate<Node2D>();
		
		AddChild(splatter);
		
		float randomRotationOffset = MyRandom.Range(_bleedingInfo.HalfRotationRandomizationRad);
		splatter.GlobalRotation = hit.Force.Angle() + randomRotationOffset;
		
		float randomizedDistanceToBleeder = _bleedingInfo.DistanceToBleeder + MyRandom.Range(_bleedingInfo.DistanceToBleederRandomization);
		Vector2 offsetFromBleeder = Vector2.FromAngle(splatter.GlobalRotation) * randomizedDistanceToBleeder;
		splatter.GlobalPosition = bleeder.GlobalPosition + offsetFromBleeder;
		
		splatter.Scale *= 1 + MyRandom.Range(_scaleRandomization);
		RandomizeColor(splatter);
		
		return new(splatter);
	}
	
	private void RandomizeColor(Node2D splatter)
	{
		float r = MyRandom.Range(1 - _bloodColorRandomization, 1);
		
		float a = _initialBloodColor.A;
		Color modulate = splatter.Modulate;
		
		modulate *= r;
		modulate.A = a;
		
		splatter.Modulate = modulate;
	}

	public override void _Process(double delta)
	{
		if (_bloodDryingTimer.IsStopped())
			return;
		
		float t = 1 - (float)(_bloodDryingTimer.TimeLeft / _bloodDryingTimer.WaitTime);
		Modulate = _initialBloodColor.Lerp(_dryBloodColor, t);
	}

	private void Simplify()
	{
		foreach (SplatterInfo splatter in _splatterList)
		{
			splatter.Node.MakeSiblingOf(this);
			splatter.Node.Modulate *= Modulate;
			_deletionTimer.Timeout += splatter.Node.QueueFree;
		}
		
		_deletionTimer.Start();
		_deletionTimer.MakeSiblingOf(this);
		QueueFree();
	}
}
