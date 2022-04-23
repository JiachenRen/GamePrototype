using UnityEngine;

namespace Terrain.Surfaces
{
    public class UnevenCubeSurface : Surface
    {
        private readonly float noiseMagnitude;
        private readonly Vector2 noiseOffset;
        private readonly float noiseScale;

        public UnevenCubeSurface(int resolution, float scale, float magnitude, Vector2 offset) : base(resolution)
        {
            noiseScale = scale;
            noiseMagnitude = magnitude;
            noiseOffset = offset;
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
            return (Mathf.PerlinNoise((x + noiseOffset.x) * noiseScale, (z + noiseOffset.y) * noiseScale) - 0.5f) *
                   noiseMagnitude;
        }
    }
}