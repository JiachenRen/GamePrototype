using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Planet : MonoBehaviour
{
    public float radius = 100;
    public int resolution = 100;
    public float waterLevelOffset = 0;

    public Material surfaceMaterial;
    public Material[] waterMaterials;

    public NoiseLayer[] noiseLayers;

    public GameObject[] agentPrototypes;

    PlanetSurface[] surfaces;
    WaterSurface[] waterSurfaces;
    UnwalkableSurface[] unwalkableMasks;

    private GameObject root;
    private List<GameObject> surfaceGameObjects;
    private List<GameObject> intermediates;

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
        waterSurfaces = new WaterSurface[6];
        unwalkableMasks = new UnwalkableSurface[6];
        surfaceGameObjects = new List<GameObject> {};
        intermediates = new List<GameObject>();
        
        var i = 0;
        foreach (var dir in directions)
        {
            surfaces[i] = MakePlanetSurface(dir);
            waterSurfaces[i] = MakeWaterSurface(dir, surfaceGameObjects[i].transform);
            unwalkableMasks[i] = MakeUnwalkableSurface(dir, surfaceGameObjects[i].transform);
            i++;
        }

        GetComponent<PlanetaryForest>().GenerateForest();

        // Everything in place. Now bake Nav Mesh.
        foreach (var obj in surfaceGameObjects)
        {
            var navMeshSurface = obj.AddComponent<NavMeshSurface>();
            navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navMeshSurface.collectObjects = CollectObjects.Children;
            navMeshSurface.BuildNavMesh();
        }

        SpawnAgents();

        // Destroy all intermediates
        foreach (var obj in intermediates)
        {
            if (!Application.isPlaying)
            {
                obj.SetActive(false);
            } else
            {
                //Destroy(obj);
            }
        }
    }

    private void SpawnAgents()
    {
        foreach (var agent in agentPrototypes)
        {
            var computerAgent = agent.GetComponent<ComputerAgent>();
            for (var i = 0; i < computerAgent.quantity; i++)
            {
                var obj = computerAgent.Spawn(RandomPositionOnNavMesh(0.3f), transform);

                // Make agent subject to planet's gravitational pull.
                GetComponent<GravityField>().subjects.Add(obj);
            }
        }
    }

    public delegate void OnRaycastHit(RaycastHit hit);

    // Get a random position on the planet where land objects can spawn.
    public Vector3 RandomPositionOnNavMesh(float elevation = 1f)
    {
        Vector3? vec = null;
        RaycastToSurface(Random.insideUnitSphere, (hit) => {
            if (hit.collider.tag == Constants.Tags.Ground && IsPointOnNavMesh(hit.point))
            {

                vec = hit.point + (hit.point - transform.position).normalized * elevation;
            }
        });
        return vec == null ? RandomPositionOnNavMesh() : vec.Value;
    }

    public bool IsPointOnNavMesh(Vector3 pos)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(pos, out hit, 1, NavMesh.AllAreas))
        {
            return true;
        }

        return false;
    }

    public bool IsAboveWater(Vector3 pos)
    {
        return (pos - transform.position).magnitude > radius + waterLevelOffset;
    }

    // Raycast from (point in space by extending from planet center in direction of norm) to planet center.
    public void RaycastToSurface(Vector3 norm, OnRaycastHit onHit)
    {
        var pointAbovePlanet = transform.position + norm * radius * 1.5f;
        RaycastHit hit;
        Ray ray = new Ray(pointAbovePlanet, -norm);

        if (Physics.Raycast(ray, out hit))
        {
            onHit(hit);
        }
    }

    private GameObject GenerateSurfaceGameObject(string name, Vector3 up)
    {
        var gameObj = new GameObject(name);
        gameObj.AddComponent<MeshFilter>();
        gameObj.transform.up = up;
        gameObj.transform.parent = root.transform;
        return gameObj;
    }

    private PlanetSurface MakePlanetSurface(Vector3 up)
    {
        var gameObj = GenerateSurfaceGameObject("Surface", up);
        gameObj.tag = Constants.Tags.Ground;
        gameObj.AddComponent<MeshRenderer>().sharedMaterial = surfaceMaterial;
        var surface = new PlanetSurface(resolution, radius, gameObj.transform, noiseLayers);
        surface.GenerateMesh();
        gameObj.AddComponent<MeshCollider>().sharedMesh = surface.mesh;
        gameObj.GetComponent<MeshFilter>().sharedMesh = surface.mesh;
        surfaceGameObjects.Add(gameObj);
        return surface;
    }

    private UnwalkableSurface MakeUnwalkableSurface(Vector3 up, Transform parent)
    {
        var gameObj = GenerateSurfaceGameObject("Unwalkable Surface", up);
        intermediates.Add(gameObj);
        gameObj.transform.parent = parent;
        gameObj.layer = Constants.Layers.TerrainMask;
        var surface = new UnwalkableSurface(resolution, radius, radius + waterLevelOffset, gameObj.transform, noiseLayers);
        surface.GenerateMesh();
        gameObj.AddComponent<MeshCollider>().sharedMesh = surface.mesh;
        gameObj.GetComponent<MeshFilter>().sharedMesh = surface.mesh;
        var mod = gameObj.AddComponent<NavMeshModifier>();
        mod.overrideArea = true;
        mod.area = Constants.AreaType.NotWalkable;
        return surface;
    }

    private WaterSurface MakeWaterSurface(Vector3 up, Transform parent)
    {
        var gameObj = GenerateSurfaceGameObject("Water Surface", Vector3.up);
        gameObj.transform.parent = parent;
        gameObj.AddComponent<MeshRenderer>().sharedMaterials = waterMaterials;
        var surface = new WaterSurface(resolution, radius + waterLevelOffset, up);
        surface.GenerateMesh();
        gameObj.GetComponent<MeshFilter>().sharedMesh = surface.mesh;
        gameObj.AddComponent<AQUAS_Lite.AQUAS_Lite_Reflection>().ignoreOcclusionCulling = true;
        var probe = gameObj.AddComponent<ReflectionProbe>();
        probe.transform.parent = gameObj.transform;
        return surface;
    }
}
