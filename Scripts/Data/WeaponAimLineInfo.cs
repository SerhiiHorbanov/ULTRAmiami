using Godot;

namespace ULTRAmiami.Data;

[GlobalClass]
public partial class WeaponAimLineInfo : Resource
{
	[Export] public Color Color { get; private set; } = Colors.Red;
	[Export] public Color NoAmmoColor { get; private set; } = Colors.Red;

	[Export] public float LineWidth { get; private set; } = 5;
	[Export] public bool ShowSpread{ get; private set; }

	[Export] public bool StopOnHit { get; private set; } = true;
}
