using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabGroup
{
    public GameObject[] prefabs;
    public float[] weights;
    public float weight = 1;

    public GameObject Sample()
    {
        if (prefabs.Length == 0)
        {
            return null;
        }

        return prefabs[WeightedSample(weights)];
    }

    public static int WeightedSample(float[] weights)
    {
        var sum = 0f;
        foreach (var weight in weights)
        {
            sum += weight;
        }

        var r = Random.value;
        var s = 0f;

        for (var i = 0; i < weights.Length; i++)
        {
            if (weights[i] <= 0f) continue;

            s +=  weights[i] / sum;
            if (s >= r) return i;
        }

        return 0;
    }
}
