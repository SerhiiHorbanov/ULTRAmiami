using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Weapons;

namespace ULTRAmiami.UI;

public partial class AmmoDisplay : Control
{
	[Export] private Label _label;
	[Export] private Unit _unit;

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
			UpdateText(0);
			return;
		}
		
		weapon.OnAmmoChanged += UpdateText;
	}

	private void StopObservingWeapon()
	{
		_unit.Weapon.OnAmmoChanged -= UpdateText;
	}

	public override void _ExitTree()
	{
		StartObservingWeapon(null);
	}

	private void UpdateText(int ammo)
	{
		_label.Text = GetAmmoText(ammo);
	}

	private string GetAmmoText(int ammo)
		=> "Ammo: " + ammo;
}
