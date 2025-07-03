using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleST : StateMachine
{
    protected SpecialForceController specialForce;
    protected ScientistController scientist;
    protected SpecialForceAnimationController specialForceAnimation;
    protected ScientistAnimationController scientistAnimation;

    private float idleDuration = 3.5f;
    private float timer = 0f;
    public IdleST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) : 
        base(agent, player, enemy, controller)
    {
        this.enemy = enemy;

        if (enemy is SpecialForceController sf)
        {
            specialForce = sf;
            specialForceAnimation = (SpecialForceAnimationController)controller;
        }
        else if (enemy is ScientistController sc)
        {
            scientist = sc;
            scientistAnimation = (ScientistAnimationController)controller;
        }
        else
        {
            Debug.LogError($"Unsupported enemy type: {enemy.GetType().Name}");
        }
    }

    public override void EnterState()
    {
        timer = 0f;
        if (specialForce != null)
        {
            specialForceAnimation.SetIdleAnimation();
        }
        else if (scientist != null)
        {
            scientistAnimation.SetIdleAnimation();
        }
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            enemy.ChangeState(new PatrolST(agent, player, enemy, controller));
            return;
        }
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();
        if (specialForce != null)
        {
            if (distanceToPlayer <= specialForce.idleAttackDistance)
            {
                if (canSeePlayer)
                {
                    enemy.ChangeState(new IdleAttackST(agent, player, enemy, specialForceAnimation));
                    return;
                }
            }
        }
        if (scientist != null)
        {
            if (distanceToPlayer <= scientist.escapeDistance)
            {
                if (canSeePlayer)
                {
                    enemy.ChangeState(new EscapeST(agent, player, enemy, scientistAnimation));
                    return;
                }
            }
        }
    }

}
