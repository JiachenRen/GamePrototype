using Audio;
using EventSystem;
using EventSystem.Events;
using Terrain;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FloatDevil : ComputerAgent
{
    public float lookRadius = 10f;
    public float attackRadius = 2f;

    public Slider slider;
    public GameObject healthBarUI;

    public GameObject player;
    public GameObject planet;

    protected override Animator anim => GetComponentInChildren<Animator>();
    
    private NavMeshAgent navMeshAgent;
    private AudioSource audioSource;

    protected void Start()
    {
        Init();
        slider.value = currentHealth / health;
        navMeshAgent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        FacePlayer();
    }

    private void Update()
    {
        if (!GameState.instance.playing) return;
        if (isDead) 
        {
            Destroy(gameObject);
            return;
        }
        
        var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        slider.value = currentHealth / health;

        if (isInjured) {
            healthBarUI.SetActive(true);
        }

        if (distanceToPlayer < attackRadius)
        {
            FacePlayer();
            if (canAttack)
            {
                Attack();
            }
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

    protected override void GetHit(Agent attacker)
    {
        base.GetHit(attacker);
        var info = new AudioSourceInfo(AudioActor.ComputerAgent, AudioAction.GetHit, TerrainType.All);
        EventManager.TriggerEvent<AudioEvent, AudioSourceInfo, AudioSource>(info, audioSource);
    }
}