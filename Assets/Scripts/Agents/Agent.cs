using UnityEngine;

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

    public bool isAttacking => animState.IsTag("Attack");
    public bool isDead => animState.IsName("Dead");

    public bool isInjured => currentHealth < health;

    protected bool canAttack => animState.IsTag("Idle") && !anim.IsInTransition(0);

    protected void Init()
    {
        currentHealth = health;
    }

    protected void Attack()
    {
        damageDealt = false;
        anim.SetTrigger(AttackTrigger);
    }

    protected virtual void HitDetection(Collision collision)
    {
        var agent = collision.gameObject.GetComponent<Agent>();
        var weapon = collision.GetContact(0).otherCollider;
        if (!weapon.CompareTag(Constants.Tags.Weapon))
            return;
        if (!agent.isAttacking)
            return;
        if (agent.damageDealt)
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
        anim.SetTrigger(DieTrigger);
    }
}