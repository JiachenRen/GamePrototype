using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FloatDevil : ComputerAgent {

    // changes these in unity inspector
    public float lookRadius = 10f; 
    public float attackRadius = 2f;

    public GameObject player;
    public GameObject planet;

    private Animator anim;
    private Animator playerAnim;
    private NavMeshAgent navMeshAgent;

    void Start() {
        anim = GetComponentInChildren<Animator>();
        playerAnim = player.GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        FacePlayer();
    }

    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < attackRadius) {
            // attack
            FacePlayer();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")) {
                anim.SetTrigger("attack");
            }
            
        } else if (distanceToPlayer < lookRadius && navMeshAgent.isOnNavMesh) {
            navMeshAgent.SetDestination(player.transform.position);
        }

        if (hp <= 0)
        {
            Die();
            Cursor.visible = true;
            Screen.lockCursor = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }

    // facing player with smooth rotation
    void FacePlayer() {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        var up = (transform.position - planet.transform.position).normalized;
        Quaternion lookAt = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, up), up);
        transform.rotation = lookAt;
    }

    // shows attack radius (red) and detection radius (cyan)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan; 
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitDetection(collision);
    }

    void HitDetection(Collision other)
    {
        bool isAttacking = playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        if (isAttacking)
        {
            if (other.gameObject.name == "Player")
            {
                print("player hit me with hand, -10HP");
                hp -= 10;
            }
            anim.SetTrigger("getHit");
        }
    }

    void Die()
    {
        anim.SetTrigger("die");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            Destroy(gameObject);
        }
    }
}
