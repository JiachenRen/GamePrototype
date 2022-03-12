using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float radius = 100;
    public int resolution = 100;
    public float waterLevelOffset = 0;

    public Material surfaceMaterial;
    public Material[] waterMaterials;

    public NoiseLayer[] noiseLayers;

    PlanetSurface[] surfaces;
    PlanetSurface[] waterSurfaces;

    private GameObject root;

    private static Vector3[] directions = { Vector3.down, Vector3.up, Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
    // Start is called before the first frame update
   public void Start()
    {
        Initialize();
    }

    public void OnValidate()
    {
        if (GetComponent<RenderInEditor>().renderingEnabled)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        if (root != null)
        {
            if (Application.isEditor)
            {
                // Optimize Performance if in editor mode.
                foreach (var surface in surfaces)
                {
                    UpdateSurface(surface, radius, noiseLayers);
                }
                foreach (var waterSurface in waterSurfaces)
                {
                    UpdateSurface(waterSurface, radius + waterLevelOffset, new NoiseLayer[] { });
                }

                return;
            } else
            {
                Destroy(root);
            }
        }

        // Delete terrains generated during editing.
        if (Application.isPlaying)
        {
            foreach (Transform t in transform)
            {
                if (t.gameObject.name == "SurfacesRoot")
                {
                    Destroy(t.gameObject);
                }
            }
        }

        root = new GameObject("SurfacesRoot");
        root.transform.parent = transform;
        surfaces = new PlanetSurface[6];
        waterSurfaces = new PlanetSurface[6];
        
        var i = 0;
        foreach (var dir in directions)
        {
            var surface = GenerateSurface(dir, radius, false);
            surface.transform.parent = root.transform;
            surfaces[i] = surface;

            var waterSurface = GenerateSurface(dir, radius + waterLevelOffset, true);
            waterSurface.transform.parent = root.transform;
            waterSurfaces[i] = waterSurface;
            GameObject obj = waterSurface.gameObject;

            obj.GetComponent<MeshRenderer>().sharedMaterials = waterMaterials;
            obj.AddComponent<AQUAS_Lite.AQUAS_Lite_Reflection>().ignoreOcclusionCulling = true;
            var probe = obj.AddComponent<ReflectionProbe>();
            probe.transform.parent = obj.transform;

            i++;
        }
    }

    private void UpdateSurface(PlanetSurface surface, float newRadius, NoiseLayer[] noiseLayers)
    {
        surface.resolution = resolution;
        surface.noiseLayers = noiseLayers;
        surface.radius = newRadius;
        surface.GenerateMesh();
    }

    private PlanetSurface GenerateSurface(Vector3 normal, float radius, bool isWaterSurface)
    {
        var gameObj = new GameObject(isWaterSurface ? "WaterSurface" : "Surface");
        gameObj.tag = Constants.Tags.Ground;
        gameObj.AddComponent<MeshFilter>();
        if (!isWaterSurface)
        {
            gameObj.AddComponent<MeshCollider>();
        }
        gameObj.AddComponent<MeshRenderer>().sharedMaterial = surfaceMaterial;
        var surface = gameObj.AddComponent<PlanetSurface>();
        surface.radius = radius;
        surface.resolution = resolution;
        surface.normal = normal;
        surface.noiseLayers = isWaterSurface ? new NoiseLayer[] { } : noiseLayers;
        surface.GenerateMesh();
        return surface;
    }

}
