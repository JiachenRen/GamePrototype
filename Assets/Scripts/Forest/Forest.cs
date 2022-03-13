using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Forest : MonoBehaviour
{
    // Start is called before the first frame update

    public PrefabCollection flora;
    public Material wind;
    public bool useWind = true;
    public int treesToSpawn = 100;

    [SerializeField, HideInInspector]
    public GameObject root;

    public void Start()
    {
        GenerateForest();
    }

    public void OnValidate()
    {
        if (GetComponent<RenderInEditor>().renderingEnabled)
        {
            GenerateForest();
        }
    }

    public void GenerateForest()
    {
        foreach (Transform t in transform)
        {
            if (Application.isPlaying && t.gameObject.name == "ForestRoot")
            {
                foreach (Transform tr in t)
                {
                    Destroy(tr.gameObject);
                }

                root = t.gameObject;
            }
        }

        if (root == null)
        {
            root = new GameObject("ForestRoot");
            root.transform.parent = transform;
        }

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
        Material material = newTree.GetComponent<MeshRenderer>().sharedMaterial;
        material.shader = useWind ? wind.shader : Shader.Find("Standard");
        return newTree;
    }

    protected abstract void SpawnTrees();
}
