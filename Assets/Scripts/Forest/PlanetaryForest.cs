using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryForest : Forest
{
    protected override void SpawnTrees()
    {
        var planet = GetComponent<Planet>();

        for (int i = 0; i < treesToSpawn; i++)
        {
            var norm = Random.insideUnitSphere;
            planet.RaycastToSurface(norm, (hit) =>
            {
                if (planet.IsAboveWater(hit.point) && hit.transform.tag == Constants.Tags.Ground)
                {
                    CreateTree(hit.point, Quaternion.FromToRotation(Vector3.up, norm)).transform.parent = hit.transform;
                }
            });
        }
    }
}
