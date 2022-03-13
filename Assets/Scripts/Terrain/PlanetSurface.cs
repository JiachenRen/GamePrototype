using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSurface : CurvedSurface
{
    public NoiseLayer[] noiseLayers = { };

    // Transform is only used to calculate seamless simplex noise.
    public Transform transform;

    public PlanetSurface(int resolution, float radius, Transform transform, NoiseLayer[] noiseLayers) : base(resolution, radius)
    {
        this.noiseLayers = noiseLayers;
        this.transform = transform;
    }

    public override float GetElevation(Vector3 normal)
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

            var h = noiseLayer.Eval(transform.rotation * normal);
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