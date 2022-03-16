using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class FloatDevil : ComputerAgent
{
    public float lookRadius = 10f;
    public float attackRadius = 2f;

    public GameObject player;
    public GameObject planet;

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Animator playerAnim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerAnim = player.GetComponent<PlayerController>().character.anim; // Todo: dynamically evaluate
        navMeshAgent = GetComponent<NavMeshAgent>();
        FacePlayer();
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < attackRadius)
        {
            // attack
            FacePlayer();
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("getHit")) anim.SetTrigger("attack");
        }
        else if (distanceToPlayer < lookRadius && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        
        if (hp <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitDetection(collision);
    }

    // shows attack radius (red) and detection radius (cyan)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRadius);
    }

    // facing player with smooth rotation
    private void FacePlayer()
    {
        var direction = (player.transform.position - transform.position).normalized;
        var up = (transform.position - planet.transform.position).normalized;
        var lookAt = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, up), up);
        transform.rotation = lookAt;
    }

    private void HitDetection(Collision other)
    {
        var isAttacking = playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        if (isAttacking)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                print("player hit me with hand, -10HP");
                hp -= 10;
            }

            anim.SetTrigger("getHit");
        }
    }

    private void Die()
    {
        anim.SetTrigger("die");
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("dead")) Destroy(gameObject);
    }
}