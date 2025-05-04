using System;
using System.Collections.Generic;
using Godot;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Controllers;

public partial class AIUnitController : UnitController
{
    protected bool IsGoing = true;
    private Queue<Vector2> _path = [];
    public Vector2 Destination { get; private set; }

    [Export] private Area2D _playerNoticingArea;

    private Vector2 _supposedDirectionToWayPoint;
    
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

        if (!IsGoing || HasNoWayPoint())
            return;
        
        if (!HasReachedWayPoint())
            return;

        SkipWayPoint();

        if (HasNoWayPoint())
            ResolveDestinationReached();
    }
    
    protected void GoTo(Vector2 newDestination)
    {
        Destination = newDestination;
        Vector2[] newPath = NavigationServer2D.Singleton.MapGetPath(GetWorld2D().GetNavigationMap(), GlobalPosition, newDestination, true);
        _path = new(newPath);
        
        if (_path.Count == 0)
            return;
        SkipWayPoint();
        IsGoing = true;
    }

    private bool HasNoWayPoint()
        => _path.Count == 0;
    
    private void SkipWayPoint()
    {
        _path.Dequeue();

        if (HasNoWayPoint())
            return;
        
        _supposedDirectionToWayPoint = (_path.Peek() - GlobalPosition).Normalized();
    }

    private bool HasReachedWayPoint()
    {
        Vector2 toWayPoint = _path.Peek() - GlobalPosition;
        float similarity = toWayPoint.CosineSimilarity(_supposedDirectionToWayPoint);
        
        return similarity < 0;
    }

    private void ResolveDestinationReached()
    {
        IsGoing = false;
        OnDestinationReached?.Invoke();
    }
    
    protected override Vector2 GetTargetDirection()
    { 
        if (!IsGoing || HasNoWayPoint())
            return Vector2.Zero;
        return _path.Peek() - Unit.GlobalPosition;
    }
}
