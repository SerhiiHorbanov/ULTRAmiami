using Godot;

namespace ULTRAmiami;

public static class MyMath
{
    public static Vector2 RotateAround(this Vector2 point, Vector2 axis, float radians)
        => (point - axis).Rotated(radians) + axis;
}