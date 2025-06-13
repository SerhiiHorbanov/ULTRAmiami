using System.Diagnostics;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami;

public partial class UnitFollowingCamera : Camera2D
{
    [Export] private Unit _unit;

    [Export] private PackedScene _deathCamera;
    
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
    
    private static UnitFollowingCamera _instance;
    private bool _isDynamic = true;
    
    public static void SetDynamic(bool isDynamic)
    {
        if (_instance is not null)
            _instance._isDynamic = isDynamic;
    }
    
    public override void _Ready()
    {
        _instance ??= this;
        
        AttachToUnit(_unit);
    }

    public override void _Process(double delta)
    {
        if (_unit is null)
            return;

        if (!_isDynamic)
        {
            GlobalPosition = _unit.GlobalPosition;
            return;
        }
        
        UpdateTargetPositionRelativeToUnit();
        DoMovement((float)delta);
    }

    public override void _ExitTree()
    {
        DetachUnit();
        
        if (_instance == this)
            _instance = null;
    }

    private void AttachToUnit(Unit unit)
    {
        if (_unit is not null)
            _unit.OnDeath -= DetachUnitFromDeath;
            
        if (unit is not null)
            unit.OnDeath += DetachUnitFromDeath;
            
        _unit = unit;
    }
    
    private void DetachUnitFromDeath(Hit hit)
    {
        DetachUnit();
        OnUnitDied(hit);
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
        
        _positionRelativeToUnit += _unit.Velocity * delta * _velocityOffsetMultiplier;
        
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

    private void OnUnitDied(Hit hit)
    {
        DeathCamera newCamera = _deathCamera.Instantiate<DeathCamera>();
        
        newCamera.MakeSiblingOf(this);
        QueueFree();
        
        newCamera.Initialize(GlobalPosition, hit);
    }
}
