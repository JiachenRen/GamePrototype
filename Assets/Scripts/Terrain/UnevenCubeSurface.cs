using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnevenCubeSurface : Surface
{
    private float noiseMagnitude;
    private float noiseScale;
    private Vector2 noiseOffset;

    public UnevenCubeSurface(int resolution, float scale, float magnitude, Vector2 offset) : base(resolution)
    {
        this.noiseScale = scale;
        this.noiseMagnitude = magnitude;
        this.noiseOffset = offset;
    }

    public override Vector3 GetVertex(float zRatio, float xRatio, Vector3 z, Vector3 x)
    {
        var zc = zRatio * 10f;
        var xc = xRatio * 10f;
        var height = GetHeightAtPoint(zc, xc);
        return Vector3.up * height + zc * z + xc * x;
    }

    private float GetHeightAtPoint(float x, float z)
    {
        return (Mathf.PerlinNoise((x + noiseOffset.x) * noiseScale, (z + noiseOffset.y) * noiseScale) - 0.5f) * noiseMagnitude;
    }
}
