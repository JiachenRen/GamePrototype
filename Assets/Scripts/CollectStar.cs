using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CollectStar : MonoBehaviour
{
    public PlayerController pc;
    public AudioSource CollectionSound;
    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
        }

        if (pc != null)
        {
            CollectionSound.Play();
            Destroy(this.gameObject);
            pc.AddHealth();
        }
    }
}
