using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloatDevil : ComputerAgent
{
    public float lookRadius = 10f;
    public float attackRadius = 2f;
    private float health;

    public Slider slider;
    public GameObject healthBarUI;

    public GameObject player;
    public GameObject planet;

    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private Animator playerAnim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        health = hp;
        slider.value = CalculateHealth();
        navMeshAgent = GetComponent<NavMeshAgent>();
        FacePlayer();
    }

    private void Update()
    {
        if (!GameState.instance.playing) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("dead")) 
        {
            Destroy(gameObject);
            return;
        }
        
        var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        
        slider.value = CalculateHealth();

        if (health < hp) {
            healthBarUI.SetActive(true);
        }

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
        var t = transform;
        var position = t.position;
        var direction = (player.transform.position - position).normalized;
        var up = (position - planet.transform.position).normalized;
        var lookAt = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, up), up);
        transform.rotation = lookAt;
    }

    private void HitDetection(Collision other)
    {
        playerAnim = player.GetComponent<PlayerController>().character.anim;
        var isAttacking = playerAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        var part = other.GetContact(0).otherCollider;
        if (!part.CompareTag("Weapon") || !isAttacking) return;

        print($"[FloatDevil] hit by {part.name}, health -= 10");
        anim.SetTrigger("getHit");
        health -= 10;

        if (health <= 0)
        {
            anim.SetTrigger("die");
        }
    }

    private float CalculateHealth() 
    {
        return health / hp;
    } 
    
    public Animator GetAnim() 
    {
        return anim;
    }
}