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
	
	public override void _Process(double delta)
	{
		_blood -= (float)delta * _bloodUsageForMaintenance;
		EmitSignalOnBloodChanged(_blood);
		
		if (_blood < 0)
			EmitSignalRunOutOfBlood(_lastHit);
	}

	public void AddBlood(float blood)
	{
		_blood += blood;
		_blood = float.Min(_blood, _max);
	}

	public void Hit(Vector2 hit)
	{
		_lastHit = hit;
		Damage(DefaultDamage);
	}
	
	private void Damage(float damage)
	{
		float newBlood = _blood - damage;
		
		if (_blood > _bloodWall)
			newBlood = float.Max(newBlood, _bloodWall);
		_blood = newBlood;
	}
}
