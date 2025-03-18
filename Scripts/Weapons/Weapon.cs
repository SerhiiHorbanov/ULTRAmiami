using Godot;
using ULTRAmiami.Units;

namespace ULTRAmiami.Weapons;

public abstract partial class Weapon : Node
{
    public Unit Unit { private get; set; }

    public Vector2 Position
        => Unit.Position;
    
    public Vector2 PointingAt { protected get; set; }
    
    public Vector2 RelativePointingAt
        => PointingAt - Position;
    
    public Vector2 PointingInDirection
        => RelativePointingAt.Normalized();
    
    public abstract void Shoot();
}