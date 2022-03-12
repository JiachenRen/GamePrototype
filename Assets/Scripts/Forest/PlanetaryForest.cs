using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryForest : Forest
{
    public Planet planet;

    public override void SpawnTrees()
    {
        for (int i = 0; i < treesToSpawn; i++)
        {
            var norm = Random.insideUnitSphere;
            var pointAbovePlanet = planet.transform.position + norm * planet.radius * 1.5f;
            RaycastHit hit;
            Ray ray = new Ray(pointAbovePlanet, -norm);

            if (Physics.Raycast(ray, out hit))
            {
                var dist = (hit.point - planet.transform.position).magnitude;
                if (dist > planet.waterLevelOffset + planet.radius && hit.transform.tag == Constants.Tags.Ground)
                {
                    CreateTree(hit.point, Quaternion.FromToRotation(Vector3.up, norm));
                }
            }
        }
    }
}
