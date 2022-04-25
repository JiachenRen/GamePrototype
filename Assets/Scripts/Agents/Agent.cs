using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Agent : MonoBehaviour
{
    private static readonly int GetHitTrigger = Animator.StringToHash("GetHit");
    private static readonly int DieTrigger = Animator.StringToHash("Die");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    
    public float health = 100;
    public float attackDamage = 10;

    private bool damageDealt = false;

    [HideInInspector] public float currentHealth;

    protected abstract Animator anim { get; }

    protected AnimatorStateInfo animState => anim.GetCurrentAnimatorStateInfo(0);

    [HideInInspector]
    public AudioSource audioSource;

    public bool isAttacking => animState.IsTag("Attack");
    public bool shouldDestroy => animState.IsName("Dead");

    public bool isInjured => currentHealth < health;

    protected bool canAttack => animState.IsTag("Idle") && !anim.IsInTransition(0);

    private bool isAlive = true;

    protected virtual void Init()
    {
        currentHealth = health;
        audioSource = GetComponent<AudioSource>();
    }

    protected void Attack()
    {
        damageDealt = false;
        anim.SetTrigger(AttackTrigger);
    }

    protected void HitDetection(Collision collision)
    {
        var agent = collision.gameObject.GetComponent<Agent>();
        var weapon = collision.GetContact(0).otherCollider;
        if (!weapon.CompareTag(Constants.Tags.Weapon))
            return;
        if (!agent.isAttacking)
            return;
        if (agent.damageDealt)
            return;
        if (!isAlive)
            return;

        Debug.Log($"{gameObject.name} hit by {weapon.name}, health -= {agent.attackDamage}");
        GetHit(agent);

        if (currentHealth <= 0) Die();
    }

    protected virtual void GetHit(Agent attacker)
    {
        currentHealth -= attacker.attackDamage;
        anim.SetTrigger(GetHitTrigger);
        attacker.damageDealt = true;
    }

    protected virtual void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        anim.SetTrigger(DieTrigger);
    }
}