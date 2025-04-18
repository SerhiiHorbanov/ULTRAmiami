using Godot;

namespace ULTRAmiami.Utils;

public static class MyMath
{
    public static Vector2 RotateAround(this Vector2 point, Vector2 axis, float radians)
        => (point - axis).Rotated(radians) + axis;
    
    public static Vector2 WithLimitedLength(this Vector2 vector, float limit)
        => vector.LengthSquared() > limit * limit ? vector.Normalized() * limit : vector;
    
    public static float CosineSimilarity(this Vector2 vector, Vector2 other)
        => vector.Normalized().Dot(other.Normalized());
}