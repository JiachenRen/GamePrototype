using System;
using UnityEditor;
using UnityEngine;

public class BullDevil : FloatDevil
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private DateTime lastTime = DateTime.UtcNow;
    
    private void Update()
    {
        if (!GameState.instance.playing) return;
        if (shouldDestroy)
        {
            Destroy(gameObject);
            return;
        }
        
        UpdateHealthBar();

        var distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer > lookRadius)
        {
            anim.SetBool(Idle, true);
            NavMeshAgent.isStopped = true;
        }

        if (distanceToPlayer < attackRadius)
        {
            if (animState.IsTag("Walk") && !anim.IsInTransition(0))
            {
                if ((DateTime.UtcNow - lastTime).Seconds > 5)
                {
                    FacePlayer();
                    NavMeshAgent.isStopped = true;
                    Attack();
                    lastTime = DateTime.UtcNow;
                }
                else
                {
                    NavMeshAgent.isStopped = false;
                    NavMeshAgent.SetDestination(player.transform.position);
                }
            }
        }
        else if (distanceToPlayer < lookRadius && NavMeshAgent.isOnNavMesh && !isAttacking && !anim.IsInTransition(0))
        {
            FacePlayer();
            anim.SetBool(Idle, false);
            NavMeshAgent.isStopped = false;
            NavMeshAgent.SetDestination(player.transform.position);
        }
    }
}