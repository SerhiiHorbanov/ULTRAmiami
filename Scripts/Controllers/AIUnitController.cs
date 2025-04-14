using System;
using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Controllers;

public partial class AIUnitController : UnitController
{
    /// IsGoingToWayPoint is set to false after reaching way point but before calling OnWayPointReached
    public bool IsGoingToWayPoint = true;
    public Vector2 CurrentWayPoint;

    [Export] private Area2D _playerNoticingArea;
    protected event Action<Unit> PlayerNoticed;
    public event Action OnWayPointReached;
    
    public override void _Ready()
    {
        base._Ready();
        _playerNoticingArea.BodyEntered += TryNoticePlayer;
    }

    private void TryNoticePlayer(Node2D body)
    {
        Unit unit = body.GetAncestor<Unit>();
        
        if (unit is null) 
            return;
        if (!unit.IsPlayer)
            return;
        GD.Print("Player Noticed");
        PlayerNoticed?.Invoke(unit);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Unit is null)
            return;
        
        if (HasReachedWayPoint((float)delta))
            ResolveCheckPointReached();
    }

    private void ResolveCheckPointReached()
    {
        IsGoingToWayPoint = false;
        OnWayPointReached?.Invoke();
    }

    private bool HasReachedWayPoint(float deltaTime)
    {
        float speedSquared = Unit.Velocity.LengthSquared();
        float distanceToWayPointSquared = Unit.GlobalPosition.DistanceTo(CurrentWayPoint);
        
        return speedSquared < distanceToWayPointSquared;
    }

    protected void GoTo(Vector2 newWayPoint)
    {
        CurrentWayPoint = newWayPoint;
        IsGoingToWayPoint = true;
    }
    
    protected override Vector2 GetTargetDirection()
        => IsGoingToWayPoint ? CurrentWayPoint - Unit.GlobalPosition : Vector2.Zero;
}
