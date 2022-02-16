using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGenerator : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ground;
    public List<GameObject> trees;
    public int treesToSpawn = 100;

    void Start()
    {
        var bounds = ground.GetComponent<Renderer>().bounds;
        var width = bounds.size.x;
        var height = bounds.size.z;

        for (int i = 0; i < treesToSpawn; i++)
        {
            var tree = trees[Random.Range(0, trees.Count)];
            var rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
            var newTree = Instantiate(tree, new Vector3(Random.Range(-width / 2, width / 2), 0, Random.Range(-width / 2, width / 2)), rotation);
            newTree.AddComponent<MeshCollider>();
        }
    }
}
