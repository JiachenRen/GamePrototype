using UnityEditor;
using UnityEngine;

public abstract class Forest : RenderInEditor
{
    // Start is called before the first frame update

    public PrefabCollection flora;
    public Material wind;
    public bool useWind = true;
    public int treesToSpawn = 100;

    [SerializeField] [HideInInspector] public GameObject root;

    public void Start()
    {
        GenerateForest();
    }

    protected override void OnEditorRender()
    {
        GenerateForest();
    }

    public void GenerateForest()
    {
        root = new GameObject("ForestRoot");
        root.transform.parent = transform;
        SpawnTrees();
    }

    protected GameObject CreateTree(Vector3 position, Quaternion rotation)
    {
        if (flora == null) return null;
        var tree = flora.Sample();
        if (tree == null) return null;
        var newTree = Instantiate(tree, position, rotation);
        newTree.AddComponent<MeshCollider>();
        newTree.transform.parent = root.transform;
        var material = newTree.GetComponent<MeshRenderer>().sharedMaterial;
        material.shader = useWind ? wind.shader : Shader.Find("Standard");
        return newTree;
    }

    protected abstract void SpawnTrees();
}