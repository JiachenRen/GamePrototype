using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground: MonoBehaviour
{
    [Tooltip("Number of vertices per row/col for the triangle mesh")]
    public int resolution = 10;
    public float maxTerrainHeight = 20;

    [Tooltip("Perlin noise scale")]
    public float perlinScale = 4;
    [Tooltip("Perlin noise offset")]
    public Vector2 perlinOffset = Vector2.zero;
    [Tooltip("Normal of the ground plane")]
    public Vector3 normal;

    public GameObject waterPlane;

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
        return (Mathf.PerlinNoise((x + perlinOffset.x) * perlinScale, (z + perlinOffset.y) * perlinScale) - 0.5f) * maxTerrainHeight;
    }

    private void GenerateMesh()
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
                var xCoor = xRatio * 10f;
                var zCoor = zRatio * 10f;
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
