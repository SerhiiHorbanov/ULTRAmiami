using System;
using Godot;
using Godot.Collections;
using ULTRAmiami.Units;
using ULTRAmiami.Utils;

namespace ULTRAmiami.Map;

public partial class Room : Node2D
{
    [Export] private Array<Node2D> _walls;
    [Export] private Vector2I _roomPosition;
    
    public MapGenerator MapGenerator;
    
    public Vector2I RoomPosition
        => _roomPosition;
    
    public void EnsureAdjacentRooms()
        => MapGenerator?.EnsureAdjacentRooms(_roomPosition);
    
    public void SetRoomPosition(Vector2I roomPosition, Vector2 roomSize)
    {
        _roomPosition = roomPosition;
        Position = roomPosition * roomSize;
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