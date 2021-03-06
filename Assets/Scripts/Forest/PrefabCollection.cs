using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class PrefabCollection
{
    public PrefabGroup[] groups;

    public GameObject Sample()
    {
        if (groups == null || groups.Length == 0) return null;

        var weights = from x in groups select x.weight;
        return groups[PrefabGroup.WeightedSample(weights.ToArray())].Sample();
    }
}