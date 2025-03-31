using System.Diagnostics;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami;

public partial class UnitFollowingCamera : Camera2D
{
    [Export] private Unit _unit;

    private Unit Unit
    {
        get => _unit;
        set
        {
            if (_unit is not null)
                _unit.OnDeath -= DetachUnit;
            
            if (value is not null)
                value.OnDeath += DetachUnit;
            
            _unit = value;
        }
    }

    [Export] private float _maxDistanceFromPlayer;
    [Export] private float _maxInputDistanceFromUnit;
    [Export] private float _relativeSpeed;
    
    private Vector2 _targetPositionRelativeToUnit;
    private Vector2 _positionRelativeToUnit = Vector2.Zero;
    
    public override void _Ready()
    {
        if (Unit is null)
            return;
        
        Unit.OnDeath += DetachUnit;
    }

    public override void _Process(double delta)
    {
        if (Unit is null)
            return;
        
        UpdateTargetPositionRelativeToUnit();
        DoMovement((float)delta);
    }

    public override void _ExitTree()
    {
        Unit.OnDeath -= DetachUnit;
    }

    private void UpdateTargetPositionRelativeToUnit()
    {
        Vector2 mousePosition = GetLocalMousePosition();

        mousePosition = mousePosition.WithLimitedLength(_maxInputDistanceFromUnit);
        
        _targetPositionRelativeToUnit = mousePosition / _maxInputDistanceFromUnit * _maxDistanceFromPlayer;
    }

    private void DoMovement(float delta)
    {
        Vector2 velocity = _targetPositionRelativeToUnit - _positionRelativeToUnit;

        velocity = velocity.WithLimitedLength(_relativeSpeed * delta);
        
        _positionRelativeToUnit +=  velocity;
        Position = Unit.Position + _positionRelativeToUnit;
    }

    private void DetachUnit()
    {
        Unit = null;
    }
}
