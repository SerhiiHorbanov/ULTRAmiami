using Godot;

namespace ULTRAmiami.Units;

public partial class BloodFuel : Node
{
	[Export] private float _blood = 0;

	public override void _Process(double delta)
	{
		GD.Print("Blood fuel: " + _blood);
	}

	public void AddBlood(float blood)
	{
		_blood += blood;
	}
}