using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public Ground ground;
    public List<GameObject> trees;
    public Material wind;
    public bool useWind = true;
    public int treesToSpawn = 100;

    [SerializeField, HideInInspector]
    private GameObject treesRoot;

    void Start()
    {
        SpawnTrees();
    }

    private void OnValidate()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        var bounds = ground.GetComponent<Renderer>().bounds;
        var width = bounds.size.x;
        var height = bounds.size.z;

        // Work-around for generated trees to show in both play and edit mode.
        if (treesRoot != null)
        {
            var treesRoot = this.treesRoot;
            if (Application.isPlaying)
            {
                Destroy(treesRoot);
            } else
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(treesRoot);
                };
            }
        }
        treesRoot = new GameObject("Trees");
        treesRoot.transform.parent = transform;
        

        for (int i = 0; i < treesToSpawn; i++)
        {
            var tree = trees[Random.Range(0, trees.Count)];
            var rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
            var x = Random.Range(-width / 2, width / 2);
            var z = Random.Range(-height / 2, height / 2);

            RaycastHit hit;
            Ray ray = new Ray(new Vector3(x, ground.maxTerrainHeight, z), Vector3.down);

            if (ground.GetComponent<MeshCollider>().Raycast(ray, out hit, ground.maxTerrainHeight * 2))
            {
                var y = hit.point.y;
                if (y > ground.waterPlane.transform.position.y)
                {
                    var newTree = Instantiate(tree, new Vector3(x, y, z), rotation);
                    newTree.AddComponent<MeshCollider>();
                    newTree.transform.parent = treesRoot.transform;
                    Material material = newTree.GetComponent<MeshRenderer>().sharedMaterial;
                    material.shader = useWind ? wind.shader : Shader.Find("Standard");
                }
            }
        }
    }
}
