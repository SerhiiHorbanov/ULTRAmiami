using System;
using Godot;

namespace ULTRAmiami;

public static class MyRandom
{
    public static Vector2 RandomVectorInRange(this Vector2 origin, Vector2 range)
    {
        float xOffset = Random.Shared.NextSingle() * range.X * 2 - range.X;
        float yOffset = Random.Shared.NextSingle() * range.Y * 2 - range.Y;

        return new(origin.X + xOffset, origin.Y + yOffset);
    }
}