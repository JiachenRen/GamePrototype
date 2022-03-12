using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSurface : Ground
{
    public float radius = 50;
    public NoiseLayer[] noiseLayers = { };

    public override Vector3 GetVertex(float xRatio, float zRatio, Vector3 xAxis, Vector3 zAxis)
    {
        var xCoord = xRatio * 2f;
        var zCoord = zRatio * 2f;
        var vertex = normal + xCoord * xAxis + zCoord * zAxis;
        var n = vertex.normalized;
        return n * (radius + GetElevation(n));
    }

    public virtual float GetElevation(Vector3 normal)
    {
        var elevation = 0f;
        var layerOutputs = new float[noiseLayers.Length];
        for (var i = 0; i < noiseLayers.Length; i++)
        {
            var noiseLayer = noiseLayers[i];
            if (!noiseLayer.enabled)
            {
                continue;
            }

            var h = noiseLayer.Eval(normal);
            layerOutputs[i] = h;
            foreach (var maskIdx in noiseLayer.maskIndices)
            {
                var maskLayer = noiseLayers[maskIdx];
                if (maskLayer.enabled && maskLayer.blendMode != NoiseLayer.BlendMode.Layer)
                {
                    layerOutputs[i] *= layerOutputs[maskIdx];
                }
            }
            if (noiseLayer.blendMode != NoiseLayer.BlendMode.Mask)
            {
                elevation += layerOutputs[i];
            }
        }
        return elevation;
    }
}