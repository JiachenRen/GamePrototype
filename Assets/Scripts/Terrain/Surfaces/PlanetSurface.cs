using UnityEngine;

namespace Terrain.Surfaces
{
    public class PlanetSurface : CurvedSurface
    {
        public NoiseLayer[] noiseLayers = { };

        // Transform is only used to calculate seamless simplex noise.
        public Transform transform;

        public PlanetSurface(int resolution, float radius, Transform transform, NoiseLayer[] noiseLayers) : base(resolution,
            radius)
        {
            this.noiseLayers = noiseLayers;
            this.transform = transform;
        }

        public override float GetElevation(Vector3 normal)
        {
            return NoiseLayer.Eval(noiseLayers, transform.rotation * normal);
        }
    }
}