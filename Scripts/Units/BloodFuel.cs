using System.Data.Common;
using Godot;
using ULTRAmiami.Units.Movement;

namespace ULTRAmiami.Units;

public partial class BloodFuel : Node
{
	[Export] private float _blood;
	[Export] private float _max;
	[Export] private float _bloodWall;
	
	[Export] private float _damageMultiplier;
	
	[Export] private float _bloodUsageForMaintenance;
	
	[Export] private float _bloodUsageForWalking;
	[Export] private UnitMovement _movement;
	
	private Hit _lastHit;

	[Signal]
	public delegate void RunOutOfBloodEventHandler(Hit lastHit);

	[Signal]
	public delegate void OnBloodChangedEventHandler(float newBlood);
	
	public override void _Process(double delta)
	{
		float bloodForMaintenance = (float)delta * GetBloodUsagePerSecond();
		
		_blood -= bloodForMaintenance;
		PlayerScore.Current.AddBloodLost(bloodForMaintenance);
		
		EmitSignalOnBloodChanged(_blood);
		
		if (_blood < 0)
			EmitSignalRunOutOfBlood(_lastHit);
	}

	public void AddBlood(float blood)
	{
		PlayerScore.Current.AddBloodConsumed(blood);
		_blood += blood;
		_blood = float.Clamp(_blood, 0, _max);
	}

	public void Hit(Hit hit)
	{
		_lastHit = hit;
		Damage(hit.Damage);
	}
	
	private void Damage(float damage)
	{
		damage *= _damageMultiplier;
		float newBlood = _blood - damage;
		PlayerScore.Current.AddBloodLost(damage);
		
		if (_blood > _bloodWall)
			newBlood = float.Max(newBlood, _bloodWall);
		_blood = newBlood;
		EmitSignalOnBloodChanged(newBlood);
	}

	private float GetBloodUsagePerSecond()
	{
		float currentWalkingBloodUsage = _movement.GetSpeedRelativeToWalkingSpeed() * _bloodUsageForWalking;
		return _bloodUsageForMaintenance + currentWalkingBloodUsage;
	}
}
