using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground: MonoBehaviour
{
    public int resolution = 20;
    public float heightVariance = 20;

    [Tooltip("Perlin noise scale")]
    public float perlinScale = 4;

    [Tooltip("Perlin noise offset")]
    public Vector2 perlinOffset = Vector2.zero;

    public GameObject waterPlane;

    public void OnValidate()
    {
        CreateSurface();
    }

    public void Start()
    {
        CreateSurface();
    }

    private void CreateSurface()
    {
        var surface = new UnevenCubeSurface(resolution, perlinScale, heightVariance, perlinOffset);
        GetComponent<MeshFilter>().sharedMesh = surface.mesh;
        GetComponent<MeshCollider>().sharedMesh = surface.mesh;
    }
}
