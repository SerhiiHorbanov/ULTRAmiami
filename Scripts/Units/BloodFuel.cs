using Godot;

namespace ULTRAmiami.Units;

public partial class BloodFuel : Node
{
	[Export] private float _blood;
	[Export] private float _max;

	[Export] private float _bloodWall;
	
	[Export] private float _bloodUsageForMaintenance;
	
	private Vector2 _lastHit;

	private const float DefaultDamage = 4;

	[Signal]
	public delegate void RunOutOfBloodEventHandler(Vector2 lastHit);

	[Signal]
	public delegate void OnBloodChangedEventHandler(float newBlood);

	private float Blood
	{
		get => _blood;
		set
		{
			float delta = value - _blood;
			if (delta < 0)
				PlayerScore.Current.AddBloodLost(-delta);
			else
				PlayerScore.Current.AddBloodConsumed(delta);
			_blood = value;
		}
	}

	public override void _Process(double delta)
	{
		Blood -= (float)delta * _bloodUsageForMaintenance;
		EmitSignalOnBloodChanged(Blood);
		
		if (Blood < 0)
			EmitSignalRunOutOfBlood(_lastHit);
	}

	public void AddBlood(float blood)
	{
		Blood += blood;
		Blood = float.Clamp(Blood, 0, _max);
	}

	public void Hit(Vector2 hit)
	{
		_lastHit = hit;
		Damage(DefaultDamage);
	}
	
	private void Damage(float damage)
	{
		float newBlood = Blood - damage;
		
		if (Blood > _bloodWall)
			newBlood = float.Max(newBlood, _bloodWall);
		Blood = newBlood;
	}
}
