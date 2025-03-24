using System;
using System.Collections.Generic;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node
{
	[Export] private bool _isAutomatic;
	[Export] private float _fireRate;

	private ulong _lastShotMicroSeconds;

	[Export] private int _ammo;
	[Export] private int _maxAmmo;

	[Export] private Node2D _dropped;
	[Export] private Area2D _droppedArea;
	private readonly Dictionary<Node2D, Unit> _unitsEnteredPickUpArea = new();
	
	public event Action<int> OnAmmoChanged;
	
	[Export] private Unit _unit;
	
	private ulong MicroSecondsBetweenShots
		=> (ulong)(1 / _fireRate * 1_000_000);
	
	private ulong MicroSecondsSinceShot
		=> Time.GetTicksUsec() - _lastShotMicroSeconds;

	private bool IsDropped
		=> _unit is null;
	
	public Vector2 Position
		=> IsDropped ? _dropped.Position : _unit.Position;
	
	public Vector2 PointingAt { protected get; set; }
	
	public Vector2 RelativePointingAt
		=> PointingAt - Position;
	
	public Vector2 PointingInDirection
		=> RelativePointingAt.Normalized();

	public override void _Ready()
	{
		SetupPickUpArea(_droppedArea);
	}

	private void SetupPickUpArea(Area2D area)
	{
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		Unit enteredUnit = body.GetAncestor<Unit>();

		if (enteredUnit == null) 
			return;
		
		_unitsEnteredPickUpArea.Add(body, enteredUnit);
		enteredUnit.EnteredDroppedWeapons.Add(this);
	}

	private void OnBodyExited(Node2D body)
	{
		if (!_unitsEnteredPickUpArea.TryGetValue(body, out Unit value))
			return;
		
        value.EnteredDroppedWeapons.Remove(this);
		_unitsEnteredPickUpArea.Remove(body);
	}
	
	public void TryAttachUnit(Unit unit, bool isPickUppable = true)
	{
		Unit prevUnit = _unit;
		_unit = unit;

		if (unit is null)
		{
			Drop(isPickUppable, prevUnit.Position);
			return;
		}
		
		RemoveChild(_dropped);
		InstantReload();
	}

	private void Drop(bool isPickUppable, Vector2 position)
	{
		_dropped.Position = position;
		
		if (isPickUppable)
		{
			AddChild(_dropped);
			return;
		}
			
		_dropped.MakeSiblingOf(this);
		QueueFree();
	}

	public void TryStartShooting()
	{
		if (ReadyToShoot())
			ShootAndDoRelatedProcesses();
	}
	
	public void TryAutomaticShooting()
	{
		if (_isAutomatic && ReadyToShoot())
			ShootAndDoRelatedProcesses();
	}

	private void InstantReload()
	{
		_ammo = _maxAmmo;
		OnAmmoChanged?.Invoke(_ammo);
	}
	
	private void ShootAndDoRelatedProcesses()
	{
		_lastShotMicroSeconds = Time.GetTicksUsec();
		Shoot();
		_ammo--;
		
		OnAmmoChanged?.Invoke(_ammo);
	}

	private bool ReadyToShoot()
	{
		return MicroSecondsSinceShot >= MicroSecondsBetweenShots && _ammo != 0;
	}

	protected abstract void Shoot();
}
