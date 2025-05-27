using System.Collections.Generic;
using Godot;
using ULTRAmiami.Data;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.UI;

public partial class AmmoDisplay : Control
{
	[Export] private Unit _unit;
	[Export] private PackedScene _ammoIconScene;
	
	private AmmoUIInfo _iconsInfo;
	
	private List<TextureRect> _ammoIcons = [];
	
	
	private Weapon ObservedWeapon
		=> _unit.Weapon;
	
	public override void _Ready()
	{
		StartObservingUnit(_unit);
	}

	public void StartObservingUnit(Unit unit)
	{
		if (unit is not null)
		{
			_unit.OnWeaponChanged -= StartObservingWeapon;
		}
		_unit = unit;

		if (unit is null)
			return;
		
		StartObservingWeapon(_unit.Weapon);
		_unit.OnWeaponChanged += StartObservingWeapon;
	}

	private void StartObservingWeapon(Weapon weapon)
	{
		if (ObservedWeapon is not null)
			StopObservingWeapon();
		
		if (weapon is null)
		{
			SetIconsAmount(0);
			return;
		}

		_iconsInfo = weapon.AmmoUIInfo;
		
		ClearIcons();
		weapon.OnAmmoChanged += SetIconsAmount;
	}

	private void StopObservingWeapon()
	{
		_unit.Weapon.OnAmmoChanged -= SetIconsAmount;
	}

	public override void _ExitTree()
	{
		StartObservingWeapon(null);
	}

	private void SetIconsAmount(int ammo)
	{
		if (ammo > _ammoIcons.Count)
		{
			ClearIcons();
			GenerateIcons(ammo);
		}
		
		while (_ammoIcons.Count > ammo)
		{
			_ammoIcons[^1].QueueFree();
			_ammoIcons.RemoveAt(_ammoIcons.Count - 1);	
		}
	}

	private void GenerateIcons(int amount)
	{
		_ammoIcons = new(amount);
		
		TextureRect current = _ammoIconScene.Instantiate<TextureRect>();
		current.Texture = _iconsInfo.Texture;
		current.Position = _iconsInfo.StartingPosition;
		current.Rotation = float.DegreesToRadians(_iconsInfo.StartingRotation);

		_ammoIcons.Add(current);
		AddChild(current);
		
		for (int i = 1; i < amount; i++)
		{
			TextureRect prev = current;
			current = _ammoIconScene.Instantiate<TextureRect>();
			current.Texture = _iconsInfo.Texture;
			
			_ammoIcons.Add(current);
			_ammoIcons[i - 1].AddChild(current);

			current.Rotation = float.DegreesToRadians(_iconsInfo.DeltaRotation);
			current.Position = _iconsInfo.DeltaPosition;
		}
	}
	
	private void ClearIcons()
	{
		foreach (TextureRect icon in _ammoIcons)
			icon.QueueFree();
		_ammoIcons = [];
	}
}
