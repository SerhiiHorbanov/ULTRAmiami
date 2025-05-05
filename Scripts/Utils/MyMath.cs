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
    
    public static Vector2? IntersectRayWithLine(Vector2 rayOrigin, Vector2 rayDir, Vector2 linePoint, Vector2 lineDir)
    {
        float determinant = rayDir.X * lineDir.Y - rayDir.Y * lineDir.X;

        // If determinant is 0, the lines are parallel
        if (Mathf.Abs(determinant) < Mathf.Epsilon)
            return null;

        Vector2 diff = linePoint - rayOrigin;

        float t = (diff.X * lineDir.Y - diff.Y * lineDir.X) / determinant;

        if (t < 0)
            return null; // Intersection is behind the ray's origin

        Vector2 intersection = rayOrigin + t * rayDir;
        return intersection;
    }
}