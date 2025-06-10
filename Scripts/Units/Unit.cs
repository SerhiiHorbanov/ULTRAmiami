using System;
using System.Collections.Generic;
using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units.Movement;
using ULTRAmiami.Utils;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.Units;

public partial class Unit : CharacterBody2D, IAttackable
{
	private Vector2 _targetDirection;
	private bool _isDead;
	
	[Export] private UnitMovement _unitMovement;
	[Export] private Weapon _weapon;
	[Export] public bool DropsPickUppableWeapon;
	[Export] public bool IgnoresAmmo;
	
	[ExportGroup("Death")]
	[Export] public bool GodMode;
	[Export] private PackedScene _deadVersion;
	[Export] private bool _isImmuneToFire;
	[Export] private HitBleedingInfo _deathFromFireBleedingInfo;
	[Export] private AudioStreamPlayer2D _deathSound;

	public readonly List<DroppedWeapon> EnteredDroppedWeapons = [];
	
	[Signal]
	public delegate void OnDeathEventHandler(Hit hit);

	[Signal]
	public delegate void OnHitEventHandler(Hit hit);
	
	[Signal]
	public delegate void OnLitUpOnFireEventHandler();

	[Signal]
	public delegate void OnWeaponChangedEventHandler(Weapon weapon);
	
	private const float DirectionDeadZoneSquared = 0.01f;
	
	public Vector2 TargetDirection
		=> _targetDirection;
	
	public Weapon Weapon
		=> _weapon;
	
	public bool IsPlayer
		=> IsInGroup(PlayerGroupName);

	public bool Bleeds
		=> true;
	
	private readonly static StringName PlayerGroupName = "Player";
	private readonly static StringName OnFireGroupName = "OnFire";
	
	public override void _Ready()
	{
		if (_weapon is not null)
		{
			Weapon weaponToAttach = _weapon;
			_weapon = null;
			TryAttachWeapon(weaponToAttach);
		}
		
		OnDeath += DropWeapon;
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	public void Hit(Hit hit)
		=> EmitSignalOnHit(hit);
	
	public void Die(Hit hit)
	{
		if (GodMode || _isDead)
			return;
		
		if (!IsPlayer)
			PlayerScore.Current.AddKill();
		
		_isDead = true;
		EmitSignalOnDeath(hit);
		DropWeapon(); 
		MakeDeadVersion();
		PlayDeathSound();
		QueueFree();
	}

	private void PlayDeathSound()
	{
		if (_deathSound is null)
			return;
		
		_deathSound.MakeSiblingOf(this);
		_deathSound.Play();
		_deathSound.GlobalPosition = GlobalPosition;
	}

	private void DieFromFire()
	{
		Die(new(Vector2.Zero, 1, _deathFromFireBleedingInfo));
	}
	
	public void TrySetOnFire()
	{
		if (IsInGroup(OnFireGroupName))
			return;
		
		AddToGroup(OnFireGroupName);
		
		if (!_isImmuneToFire)
			EmitSignalOnLitUpOnFire();
	}
	
	private void MakeDeadVersion()
	{
		Node2D deadVersion = _deadVersion?.Instantiate<Node2D>();

		if (deadVersion is null)
			return;
		
		deadVersion.MakeSiblingOf(this);
		deadVersion.Position = Position;
	}

	public void SetTargetDirection(Vector2 targetDirection)
	{
		_unitMovement.SetTargetDirection(targetDirection);
		if (!_targetDirection.IsNormalized())
			targetDirection = targetDirection.Normalized();
		
		_targetDirection = targetDirection;
	}

	public void SetPointingAt(Vector2 pointingAt)
	{
		_weapon.PointingAt = pointingAt;
	}

	public void DropWeapon(Hit hit)
		=> DropWeapon();
	public void DropWeapon()
		=> TryAttachWeapon(null);

	public void PickUpWeapon()
	{
		if (EnteredDroppedWeapons.Count == 0)
			return;
		
		Weapon closestWeapon = GetClosestEnteredDroppedWeapon();

		TryAttachWeapon(closestWeapon);
	}

	private Weapon GetClosestEnteredDroppedWeapon()
	{
		DroppedWeapon closestDroppedWeapon = EnteredDroppedWeapons[0];
		float closestDistanceSquared = Position.DistanceSquaredTo(EnteredDroppedWeapons[0].Position);
		
		foreach (DroppedWeapon weapon in EnteredDroppedWeapons)
		{
			float distanceSquared = Position.DistanceSquaredTo(weapon.Position);
			if (distanceSquared < closestDistanceSquared)
			{
				closestDroppedWeapon = weapon;
				closestDistanceSquared = distanceSquared;
			}
		}

		return closestDroppedWeapon.Weapon;
	}
	
	private void TryAttachWeapon(Weapon weapon)
	{
		if (ReferenceEquals(_weapon, weapon))
			return;

		Weapon oldWeapon = _weapon;
		_weapon = weapon;
		
		EmitSignalOnWeaponChanged(weapon);
		
		oldWeapon?.TryAttachUnit(null, DropsPickUppableWeapon);
		weapon?.TryAttachUnit(this);
	}
}
