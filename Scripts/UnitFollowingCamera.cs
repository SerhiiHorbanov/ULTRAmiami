using System.Diagnostics;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami;

public partial class UnitFollowingCamera : Camera2D
{
    [Export] private Unit _unit;

    [Export] private float _maxDistanceFromPlayer;
    [Export] private float _maxInputDistanceFromUnit;
    [Export] private float _linearMovementSpeed;
    [Export] private float _lerpMovementSpeed;
    [Export] private float _linearMovementThreshold;
    [Export] private float _velocityOffsetMultiplier;
    
    private Vector2 _targetPositionRelativeToUnit;
    private Vector2 _positionRelativeToUnit = Vector2.Zero;

    private Vector2 TargetPosition
        => _unit.Position + _targetPositionRelativeToUnit;

    private float LinearMovementThresholdSquared
        => _linearMovementThreshold * _linearMovementThreshold;
    
    public override void _Ready()
    {
        AttachToUnit(_unit);
    }

    public override void _Process(double delta)
    {
        if (_unit is null)
            return;
        
        UpdateTargetPositionRelativeToUnit();
        DoMovement((float)delta);
    }

    public override void _ExitTree()
    {
        DetachUnit();
    }

    private void AttachToUnit(Unit unit)
    {
        if (_unit is not null)
            _unit.OnDeath -= DetachUnit;
            
        if (unit is not null)
            unit.OnDeath += DetachUnit;
            
        _unit = unit;
    }
    
    private void DetachUnit()
        => AttachToUnit(null);
    
    private void UpdateTargetPositionRelativeToUnit()
    {
        Vector2 mousePosition = GetLocalMousePosition();
        
        mousePosition = mousePosition.WithLimitedLength(_maxInputDistanceFromUnit);
        
        _targetPositionRelativeToUnit = mousePosition / _maxInputDistanceFromUnit * _maxDistanceFromPlayer;
    }

    private void DoMovement(float delta)
    {
        if (Position.DistanceSquaredTo(TargetPosition) > LinearMovementThresholdSquared)
            DoLerpMovement(delta);
        else
            DoLinearMovement(delta);
        
        _positionRelativeToUnit += _unit.Velocity * _velocityOffsetMultiplier;
        
        UpdatePosition();
    }

    private void DoLerpMovement(float delta)
    {
        float lerpWeight = float.Min(_lerpMovementSpeed * delta, 1);
        
        _positionRelativeToUnit = _positionRelativeToUnit.Lerp(_targetPositionRelativeToUnit, lerpWeight);
    }

    private void DoLinearMovement(float delta)
    {
        Vector2 velocity = _targetPositionRelativeToUnit - _positionRelativeToUnit;

        velocity = velocity.WithLimitedLength(_linearMovementSpeed * delta);
        _positionRelativeToUnit += velocity;
    }
    
    private void UpdatePosition()
    {
        Position = _positionRelativeToUnit + _unit.Position;
    }
}
