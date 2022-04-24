using Audio;
using EventSystem;
using EventSystem.Events;
using Terrain;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class ComputerAgent : Agent
{
    // Number of agents to spawn.
    public int quantity;

    public GameObject prototype;

    public PrefabCollection itemDrops;
    public GameObject Spawn(Vector3 position, Transform parent)
    {
        var obj = Instantiate(prototype, parent);
        obj.GetComponent<NavMeshAgent>().Warp(position);
        return obj;
    }
    
    protected override void GetHit(Agent attacker)
    {
        base.GetHit(attacker);
        var info = new AudioSourceInfo(AudioActor.ComputerAgent, AudioAction.GetHit, TerrainType.All);
        EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(info, AudioSource);
    }
    
    protected override void Die()
    {
        base.Die();
        var info = new AudioSourceInfo(AudioActor.ComputerAgent, AudioAction.Die, TerrainType.All);
        EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(info, AudioSource);
        EventManager.TriggerEvent<AgentDeathEvent, ComputerAgent>(this);
        if (itemDrops != null)
        {
            var sample = itemDrops.Sample();
            if (sample != null)
            {
                var t = transform;
                Instantiate(sample, t.position + t.up * 1f, t.rotation);
            }
        }
    }
}