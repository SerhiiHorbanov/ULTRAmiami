using Godot;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class UnitAnimator : Node2D
{
	[Export] private Unit _unit;
	[Export] private Sprite2D _bodySprite;
	[Export] private AnimatedSprite2D _legs;

	private Weapon Weapon
		=> _unit.Weapon;
	
	private bool IsWalking
		=> _unit.TargetDirection != Vector2.Zero;
	
	public override void _Process(double delta)
	{
		UpdateRotation();
		UpdateAnimation();
	}

	private void UpdateAnimation()
	{
		if (IsWalking)
			_legs.Play();
		else
			_legs.Stop();
	}

	private void UpdateRotation()
	{
		if (Weapon is null)
			_bodySprite.Rotation = _legs.Rotation;
		else
			_bodySprite.Rotation = Weapon.RelativePointingAt.Angle();
		
		if (IsWalking)
			_legs.Rotation = _unit.TargetDirection.Angle();
		else
			_legs.Rotation = _bodySprite.Rotation;
	}
}
