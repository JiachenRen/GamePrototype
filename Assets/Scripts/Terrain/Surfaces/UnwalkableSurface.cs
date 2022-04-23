using UnityEngine;

namespace Terrain.Surfaces
{
    public class UnwalkableSurface : PlanetSurface
    {
        // Maximum depth allowed under water.
        public float allowedDepthUnderWater = 1;
        public float waterRadius;

        public UnwalkableSurface(int resolution, float radius, float waterRadius, Transform transform,
            NoiseLayer[] noiseLayers) : base(resolution, radius, transform, noiseLayers)
        {
            this.waterRadius = waterRadius;
        }

        public override Vector3 GetVertex(float zr, float xr, Vector3 z, Vector3 x)
        {
            var zc = zr * 2f;
            var xc = xr * 2f;
            var vertex = Vector3.up + zc * z + xc * x;
            var n = vertex.normalized;
            var height = radius + GetElevation(n);
            var threshold = waterRadius - allowedDepthUnderWater;
            return n * (height < threshold ? height : threshold);
        }
    }
}