using UnityEngine;

public class PlanetaryForest : Forest
{
    protected override void SpawnTrees()
    {
        var planet = GetComponent<Planet>();

        for (var i = 0; i < treesToSpawn; i++)
        {
            var norm = Random.insideUnitSphere;
            planet.RaycastToSurface(norm, hit =>
            {
                if (planet.IsAboveWater(hit.point) && hit.transform.CompareTag(Constants.Tags.Ground))
                    CreateTree(hit.point, Quaternion.FromToRotation(Vector3.up, norm)).transform.parent = hit.transform;
            });
        }
    }
}