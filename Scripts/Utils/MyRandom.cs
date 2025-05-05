using System;
using Godot;

namespace ULTRAmiami.Utils;

public static class MyRandom
{
    public static Vector2 RandomVectorInRange(this Vector2 origin, Vector2 range)
    {
        float xOffset = Random.Shared.NextSingle() * range.X * 2 - range.X;
        float yOffset = Random.Shared.NextSingle() * range.Y * 2 - range.Y;

        return new(origin.X + xOffset, origin.Y + yOffset);
    }

    public static float Range(float distanceFrom0)
        => Range(-distanceFrom0, distanceFrom0);
    
    public static float Range(float limit1, float limit2)
    {
        if (limit1 > limit2)
            (limit1, limit2) = (limit2, limit1);
        return Random.Shared.NextSingle() * (limit2 - limit1) + limit1;
    }
}