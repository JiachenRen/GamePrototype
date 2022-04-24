using Audio;
using EventSystem;
using EventSystem.Events;
using Terrain;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BullDevil : ComputerAgent {

    public float lookRadius = 20f;
    public float attackRadius = 4f;

    public GameObject player;
    public GameObject planet;

    protected override Animator anim => GetComponentInChildren<Animator>();

    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;

    private System.DateTime lastTime = System.DateTime.UtcNow;

    protected void Start() {
        Init();
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        FacePlayer();
        //gameObject.SetActive(false);
    }

    private void Update() {
        if (!GameState.instance.playing) return;
        if (shouldDestroy) {
            Destroy(gameObject);
            return;
        }

        var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer > lookRadius) {
            anim.SetBool("Idle", true);
            navMeshAgent.isStopped = true;
        }
        
        if (distanceToPlayer < attackRadius) {
            if (animState.IsTag("Walk") && !anim.IsInTransition(0)) {
                if ((System.DateTime.UtcNow - lastTime).Seconds > 5) {
                    FacePlayer();
                    navMeshAgent.isStopped = true;
                    Attack();
                    lastTime = System.DateTime.UtcNow;
                } else {
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(player.transform.position);
                }
            }
        } else if (distanceToPlayer < lookRadius && navMeshAgent.isOnNavMesh && !isAttacking && !anim.IsInTransition(0)) {
            FacePlayer();
            anim.SetBool("Idle", false);
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        HitDetection(collision);
    }

    // shows attack radius (red) and detection radius (cyan)
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, attackRadius);
    }

    // facing player with smooth rotation
    private void FacePlayer() {
        var t = transform;
        var position = t.position;
        var direction = (player.transform.position - position).normalized;
        var up = (position - planet.transform.position).normalized;
        var lookAt = Quaternion.LookRotation(Vector3.ProjectOnPlane(direction, up), up);
        transform.rotation = lookAt;
    }

}
