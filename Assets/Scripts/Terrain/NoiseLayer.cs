using System;
using UnityEngine;

namespace Terrain
{
    [Serializable]
    public class NoiseLayer
    {
        public enum BlendMode
        {
            Layer,
            Mask,
            LayerAndMask
        }

        public BlendMode blendMode = BlendMode.LayerAndMask;
        public float frequency = 1;
        public float amplitude = 1;
        public Vector3 offset;

        [Tooltip("Layer indices must be smaller than or equal to index of this layer.")]
        public int[] maskIndices;

        public bool enabled = true;
        private Noise noise;

        public NoiseLayer()
        {
            noise = new Noise();
        }

        // Evaluate Simplex noise at point.
        public float Eval(Vector3 unitVec)
        {
            return (noise.Evaluate(unitVec * frequency + offset) + 1) / 2 * amplitude;
        }

        public static float Eval(NoiseLayer[] layers, Vector3 norm)
        {
            var val = 0f;
            var layerOutputs = new float[layers.Length];
            for (var i = 0; i < layers.Length; i++)
            {
                var noiseLayer = layers[i];
                if (!noiseLayer.enabled) continue;

                var h = noiseLayer.Eval(norm);
                layerOutputs[i] = h;
                foreach (var maskIdx in noiseLayer.maskIndices)
                {
                    var maskLayer = layers[maskIdx];
                    if (maskLayer.enabled && maskLayer.blendMode != BlendMode.Layer)
                        layerOutputs[i] *= layerOutputs[maskIdx];
                }

                if (noiseLayer.blendMode != BlendMode.Mask) val += layerOutputs[i];
            }

            return val;
        }
    }
}