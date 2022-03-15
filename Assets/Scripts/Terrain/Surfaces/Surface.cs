using UnityEngine;

namespace Terrain.Surfaces
{
    public abstract class Surface
    {
        public Mesh mesh;
        public int resolution = 10;

        public Surface(int resolution)
        {
            this.resolution = resolution;
        }

        public virtual void GenerateMesh()
        {
            if (mesh == null) mesh = new Mesh();

            var vertices = new Vector3[resolution * resolution];
            var triangles = new int[6 * (resolution - 1) * (resolution - 1)];
            var index = 0;
            var i = 0;
            for (var x = 0; x < resolution; x++)
            for (var z = 0; z < resolution; z++)
            {
                var zRatio = z / (resolution - 1f) - 0.5f;
                var xRatio = x / (resolution - 1f) - 0.5f;
                vertices[i] = GetVertex(zRatio, xRatio, Vector3.forward, Vector3.right);

                if (z < resolution - 1 && x < resolution - 1)
                {
                    triangles[index] = i;
                    triangles[index + 1] = i + resolution + 1;
                    triangles[index + 2] = i + resolution;

                    triangles[index + 3] = i;
                    triangles[index + 4] = i + 1;
                    triangles[index + 5] = i + resolution + 1;

                    index += 6;
                }

                i++;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        public abstract Vector3 GetVertex(float zRatio, float xRatio, Vector3 z, Vector3 x);
    }
}