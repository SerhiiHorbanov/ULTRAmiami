using System.Collections.Generic;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Weapons;

public partial class DroppedWeapon : Node2D
{
    [Export] private Weapon _weapon;
    
    [Export] private Area2D _droppedArea;
    private readonly Dictionary<Node2D, Unit> _unitsEnteredPickUpArea = new();

    [Export] private ShaderMaterial _pickuppableWeaponMaterial;
    [Export] private Node2D _nodeApplyingMaterialTo;
    
    public Weapon Weapon
        => _weapon;

    public void DetachWeapon()
        => _weapon = null;
    
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
        if (enteredUnit.IsPlayer)
            _nodeApplyingMaterialTo.Material = _pickuppableWeaponMaterial;
    }

    private void OnBodyExited(Node2D body)
    {
        if (!_unitsEnteredPickUpArea.TryGetValue(body, out Unit value))
            return;
		
        value.EnteredDroppedWeapons.Remove(this);
        _unitsEnteredPickUpArea.Remove(body);
        
        if (value.IsPlayer)
            _nodeApplyingMaterialTo.Material = null;
    }
}
