using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ULTRAmiami.Units;

namespace ULTRAmiami.Map;

//Vector2I room positions are not in global coordinates, but in amount of rooms from origin
public partial class MapGenerator : Node
{
    [Export] private Array<Room> _rooms = [];
    
    [Export] private uint _roomsTargetAmount;
    [Export] private float _allowedDistanceToDeletingRoom;

    [Export] private Vector2 _roomSize;

    [Export] private Room _initialRoom;
    [Export] private Array<PackedScene> _packedRooms;
    [Export] private Unit _unit;
    
    private float AllowedDistanceSquaredToDeletingRoom
        => _allowedDistanceToDeletingRoom * _allowedDistanceToDeletingRoom;

    private Vector2 KeptRoomsPosition
        => _unit?.Position ?? Vector2.Zero;

    public override void _Ready()
    {
        _unit.OnDeath += _ => _unit = null; 
        _initialRoom.MapGenerator = this;
        _rooms.Add(_initialRoom);
        _initialRoom = null;
        
        EnsureRoomAndAdjacent(Vector2I.Zero);
    }

    public void EnsureRoomAndAdjacent(Vector2I roomPosition)
    {
        EnsureRoomExistsAtPosition(roomPosition);
        EnsureRoomExistsAtPosition(roomPosition + Vector2I.Right);
        EnsureRoomExistsAtPosition(roomPosition + Vector2I.Left);
        EnsureRoomExistsAtPosition(roomPosition + Vector2I.Down);
        EnsureRoomExistsAtPosition(roomPosition + Vector2I.Up);
    }

    private void EnsureRoomExistsAtPosition(Vector2I roomPosition)
    {
        if (GetRoom(roomPosition) is not null)
            return;
        
        Room newRoom = _packedRooms.PickRandom().Instantiate<Room>();
        newRoom.SetRoomPosition(roomPosition, _roomSize);
        newRoom.MapGenerator = this;
        CallDeferred(Node.MethodName.AddChild, newRoom);
        _rooms.Add(newRoom);
        
        TryEnforceTargetRoomAmount();
        
        foreach (Room eachRoom in _rooms)
            newRoom.EnsureWallsDontConflict(eachRoom);
    }

    private Room GetRoom(Vector2I roomPosition)
    {
        foreach (Room room in _rooms)
        {
            if (room.RoomPosition == roomPosition)
                return room;
        }
        
        return null;
    }

    private void TryEnforceTargetRoomAmount()
    {
        if (_rooms.Count < _roomsTargetAmount)
            return;
        
        for (int i = 0; i < _rooms.Count; i++)
        {
            Vector2 roomGlobalPosition = _rooms[i].RoomPosition * _roomSize;
            
            if (roomGlobalPosition.DistanceSquaredTo(KeptRoomsPosition) > AllowedDistanceSquaredToDeletingRoom)
            {
                _rooms[i].QueueFree();
                _rooms.RemoveAt(i);
                
                if (_rooms.Count <= _roomsTargetAmount)
                    return;
            }
        }
    }
}
