using System;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Controllers;

public partial class AIUnitController : UnitController
{
    protected bool IsGoing = true;

    [Export] private Area2D _playerNoticingArea;
    [Export] private NavigationAgent2D _navAgent;
    
    protected event Action<Unit> PlayerNoticed;
    public event Action OnDestinationReached;
    
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
        PlayerNoticed?.Invoke(unit);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Unit is null)
            return;
        
        if (HasReachedDestination((float)delta))
            ResolveDestinationReached();
    }

    private void ResolveDestinationReached()
    {
        IsGoing = false;
        OnDestinationReached?.Invoke();
    }

    private bool HasReachedDestination(float deltaTime)
    {
        float speedSquared = Unit.Velocity.LengthSquared();
        float distanceToDestinationSquared = Unit.GlobalPosition.DistanceTo(_navAgent.TargetPosition);
        
        return speedSquared < distanceToDestinationSquared;
    }

    protected void GoTo(Vector2 newDestination)
    {
        _navAgent.TargetPosition = newDestination;
        IsGoing = true;
    }
    
    protected override Vector2 GetTargetDirection()
        => IsGoing ? _navAgent.GetNextPathPosition() - Unit.GlobalPosition : Vector2.Zero;
}
