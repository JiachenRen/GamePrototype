using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerAgent : Agent
{
    // Number of agents to spawn.
    public int quantity;

    public GameObject prototype;

    public GameObject Spawn(Vector3 position, Transform parent)
    {
        var obj = Instantiate(prototype, parent);
        obj.GetComponent<NavMeshAgent>().Warp(position);
        return obj;
    }
}
