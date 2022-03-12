using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseLayer
{
    public BlendMode blendMode = BlendMode.LayerAndMask;
    public float frequency = 1;
    public float amplitude = 1;
    public Vector3 offset;

    [Tooltip("Layer indices must be smaller than or equal to index of this layer.")]
    public int[] maskIndices;
    public bool enabled = true;
    private Noise noise;


    public enum BlendMode
    {
        Layer, Mask, LayerAndMask
    }

    public NoiseLayer()
    {
        noise = new Noise();
    }
    // Evaluate Simplex noise at point.
    public float Eval(Vector3 unitVec)
    {
        return (noise.Evaluate(unitVec * frequency + offset) + 1) / 2 * amplitude;
    }
}