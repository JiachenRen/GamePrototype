using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : CurvedSurface
{
    public Vector3 normal;
    public WaterSurface(int resolution, float radius, Vector3 normal) : base(resolution, radius)
    {
        this.normal = normal;
    }

    public override Vector3 GetVertex(float zr, float xr, Vector3 z, Vector3 x)
    {
        return Quaternion.FromToRotation(Vector3.up, normal) * base.GetVertex(zr, xr, z, x);
    }
}
