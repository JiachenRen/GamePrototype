using System;
using Player;
using UnityEngine;


namespace Potion
{
    [RequireComponent(typeof(Collider))]
    public abstract class Potion : MonoBehaviour
    {
        private bool effectsApplied;
        public abstract void Apply(Agent agent);

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            var player = other.GetComponentInParent<PlayerController>();
            if (player != null && !effectsApplied)
            {
                Apply(player);
                effectsApplied = true;
                Destroy(gameObject);
            }
        }
    }
}

