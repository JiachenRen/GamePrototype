using Terrain;
using UnityEngine;

public class PlanarForest : Forest
{
    public Ground ground;

    protected override void SpawnTrees()
    {
        var bounds = ground.GetComponent<Renderer>().bounds;
        var width = bounds.size.x;
        var height = bounds.size.z;

        for (var i = 0; i < treesToSpawn; i++)
        {
            var tree = flora.Sample();
            var rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, Random.Range(0, 180), 0);
            var x = Random.Range(-width / 2, width / 2);
            var z = Random.Range(-height / 2, height / 2);

            RaycastHit hit;
            var ray = new Ray(new Vector3(x, ground.heightVariance, z), Vector3.down);

            if (ground.GetComponent<MeshCollider>().Raycast(ray, out hit, ground.heightVariance * 2))
            {
                var y = hit.point.y;
                if (y > ground.waterPlane.transform.position.y) CreateTree(new Vector3(x, y, z), rotation);
            }
        }
    }
}