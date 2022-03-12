using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatDevilAI : MonoBehaviour {

    // changes these in unity inspector
    public float lookRadius = 10f; 
    public float attackRadius = 2f;
    public GameObject player;

    private Animator anim;

    void Start() {
        anim = GetComponentInChildren<Animator>();
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < attackRadius) {
            // attack
            FacePlayer();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")) {
                anim.SetTrigger("attack");
            }
            
        } else if (distanceToPlayer < lookRadius) {
            // navigate
            FacePlayer();
            // ----------- TODO: Move towards player -----------

            // ----------- END TODO -----------
        }
    }

    // facing player with smooth rotation
    void FacePlayer() {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, Time.deltaTime * 5f);
    }

    // shows attack radius (red) and detection radius (cyan)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan; 
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
