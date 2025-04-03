using System;
using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami.Map;

public partial class Room : Node2D
{
    [Export] private Array<Node2D> _walls;
    private Vector2I _roomPosition = Vector2I.Zero;
    
    [Export] private Area2D _rightDoorTrigger;
    [Export] private Area2D _leftDoorTrigger;
    [Export] private Area2D _bottomDoorTrigger;
    [Export] private Area2D _topDoorTrigger;
    
    public MapGenerator MapGenerator;
    
    public Vector2I RoomPosition
        => _roomPosition;

    public override void _Ready()
    {
        _rightDoorTrigger.BodyEntered += EnsureRightRoomAndAdjacent;
        _leftDoorTrigger.BodyEntered += EnsureLeftRoomAndAdjacent;
        _bottomDoorTrigger.BodyEntered += EnsureBottomRoomAndAdjacent;
        _topDoorTrigger.BodyEntered += EnsureTopRoomAndAdjacent;
    }

    private void EnsureRightRoomAndAdjacent(Node2D node)
        => EnsureRoomAndAdjacentByRelativePositionIfNodeIsUnit(Vector2I.Right, node);
    private void EnsureLeftRoomAndAdjacent(Node2D node)
        => EnsureRoomAndAdjacentByRelativePositionIfNodeIsUnit(Vector2I.Left, node);
    private void EnsureBottomRoomAndAdjacent(Node2D node)
        => EnsureRoomAndAdjacentByRelativePositionIfNodeIsUnit(Vector2I.Down, node);
    private void EnsureTopRoomAndAdjacent(Node2D node)
        => EnsureRoomAndAdjacentByRelativePositionIfNodeIsUnit(Vector2I.Up, node);

    public void SetRoomPosition(Vector2I roomPosition, Vector2 roomSize)
    {
        _roomPosition = roomPosition;
        Position = roomPosition * roomSize;
    }
    
    private void EnsureRoomAndAdjacentByRelativePositionIfNodeIsUnit(Vector2I relative, Node2D node)
    {
        if (node.GetAncestor<Unit>() is null)
            return;
        
        MapGenerator?.EnsureRoomAndAdjacent(_roomPosition + relative);
    }

    public bool HasConflictingWall(Node2D wall, out Node2D conflictingWall)
    {
        conflictingWall = null;
        foreach (Node2D myWall in _walls)
        {
            if (SameWall(myWall, wall))
            {
                conflictingWall = myWall;
                return true;
            }
        }

        return false;
    }

    public void EnsureWallsDontConflict(Room room)
    {
        if (!IsRoomAdjacent(room))
            return;

        foreach (Node2D wall in room._walls)
        {
            if (HasConflictingWall(wall, out Node2D conflictingWall))
                conflictingWall.QueueFree();
        }
    }

    private bool IsRoomAdjacent(Room room)
    {
        Vector2I d = (room._roomPosition - _roomPosition).Abs();
        return ((d.X == 0) != (d.Y == 0)) && d.X <= 1 && d.Y <= 1;
    }

    private bool SameWall(Node2D wall, Node2D otherWall)
        => wall.GlobalPosition.DistanceSquaredTo(otherWall.GlobalPosition) < 1;
}