using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground: MonoBehaviour
{
    public int resolution = 10;
    public float scale = 10;
    public Vector2 offset = Vector2.zero;
    public float maxTerrainHeight = 20;
    public Vector3 normal;

    public void Start()
    {
        GenerateMesh();
    }

    public void OnValidate()
    {
        GenerateMesh();
    }

    public float GetHeightAtPoint(float x, float z)
    {
        return (Mathf.PerlinNoise((x + offset.x) * scale, (z + offset.y) * scale) - 0.5f) * maxTerrainHeight;
    }

    public void GenerateMesh()
    {
        var mesh = new Mesh();
        Vector3 xAxis = new Vector3(normal.y, normal.z, normal.x);
        Vector3 zAxis = Vector3.Cross(normal, xAxis);

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[6 * (resolution - 1) * (resolution - 1)];
        int index = 0;
        int i = 0;
        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float xRatio = x / (resolution - 1f) - 0.5f;
                float zRatio = z / (resolution - 1f) - 0.5f;
                var xCoor = xRatio * 2f;
                var zCoor = zRatio * 2f;
                var height = GetHeightAtPoint(xCoor, zCoor);
                var vertex = normal * height + xCoor * xAxis + zCoor * zAxis;
                vertices[i] = vertex;

                if (x < resolution - 1 && z < resolution - 1)
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
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Has to be done after mesh is generated.
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}
