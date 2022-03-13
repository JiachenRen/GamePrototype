using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSurface : Surface
{
    public float radius = 50;

    public CurvedSurface(int resolution, float radius) : base(resolution)
    {
        this.radius = radius;
    }

    public override Vector3 GetVertex(float zr, float xr, Vector3 z, Vector3 x)
    {
        var zc = zr * 2f;
        var xc = xr * 2f;
        var vertex = Vector3.up + zc * z + xc * x;
        var n = vertex.normalized;
        return n * (radius + GetElevation(n));
    }

    public virtual float GetElevation(Vector3 normal)
    {
        return 0;
    }
}