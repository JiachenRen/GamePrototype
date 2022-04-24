using UnityEngine;

namespace Terrain
{
    [System.Serializable]
    public class NoiseConfig
    {
        public Vector3 offset;
        
        [Min(0)]
        [Tooltip("How rough is the terrain.")]
        public float baseFrequency = 1;
        
        [Min(0)]
        [Tooltip("Frequency multiplier per sub-layer.")]
        public float frequency = 2;
        
        [Range(0, 1)]
        [Tooltip("How much amplitude decays per sub-layer.")]
        public float decay = 0.5f;
        
        [Min(0)]
        [Tooltip("Scale/norm of the noise.")]
        public float scale = 1;

        [Min(0)]
        [Tooltip("Only noise above the threshold is considered. Resulting noise val = (val - threshold).")]
        public float threshold = 0;
        
        [Range(1, 8)]
        public int subLayers = 1;
    }
}