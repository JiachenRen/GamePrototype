using Audio;
using EventSystem;
using EventSystem.Events;
using Player;
using Terrain;
using UnityEngine;

namespace Potion
{
    [RequireComponent(typeof(Collider))]
    public abstract class Potion : MonoBehaviour
    {
        private bool effectsApplied;

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null && !effectsApplied)
            {
                Apply(player);
                var sourceInfo = new AudioSourceInfo(AudioActor.Player, AudioAction.DrinkPotion, TerrainType.All);
                EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(sourceInfo,
                    player.audioSource);
                effectsApplied = true;
                Destroy(gameObject);
            }
        }

        public abstract void Apply(Agent agent);
    }
}