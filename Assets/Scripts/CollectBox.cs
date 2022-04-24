using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CollectBox : MonoBehaviour
{
    public PlayerController pc;
    public AudioSource HitBox;

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            pc = c.attachedRigidbody.gameObject.GetComponent<PlayerController>();
        }

        if (pc != null)
        {
            HitBox.Play();
            Destroy(this.gameObject);
        }
    }
}
