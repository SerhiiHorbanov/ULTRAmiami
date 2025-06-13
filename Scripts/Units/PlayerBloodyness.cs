using Godot;

namespace ULTRAmiami.Units;

public partial class PlayerBloodyness : Node
{
	public float Bloodyness { get; private set; }
	[Export] private float _lossPerSecond;
	[Export] private float _maxBloodyness;
	
	public override void _Process(double delta)
	{
		Bloodyness -= _lossPerSecond * (float)delta;
		Bloodyness = float.Clamp(Bloodyness, 0, _maxBloodyness);
	}

	private void OnBloodGained(float value)
	{
		Bloodyness += value;
	}
}
