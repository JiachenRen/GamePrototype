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
        
        [Tooltip("Layer indices must be smaller than or equal to index of this layer.")]
        public int[] maskIndices;

        public bool enabled = true;
        
        public NoiseConfig noiseConfig;
        
        private Noise noise;

        public NoiseLayer()
        {
            noise = new Noise();
        }

        // Evaluate Simplex noise at point.
        public float Eval(Vector3 unitVec)
        {
            var noiseVal = 0f;
            var freq = noiseConfig.baseFrequency;
            var amp = 1f;

            for (var i = 0; i < noiseConfig.subLayers; i++)
            {
                var val = noise.Evaluate(unitVec * freq + noiseConfig.offset);
                noiseVal += (val + 1) * amp / 2;
                freq *= noiseConfig.frequency;
                amp *= noiseConfig.decay;
            }

            noiseVal = Mathf.Max(0, noiseVal - noiseConfig.threshold);
            return noiseVal * noiseConfig.scale;
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