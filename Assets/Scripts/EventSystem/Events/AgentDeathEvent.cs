using Audio;
using Player;
using Terrain;
using UnityEngine;
using UnityEngine.Events;

namespace EventSystem.Events
{
    public class AgentDeathEvent: UnityEvent<ComputerAgent>
    {
    }
}