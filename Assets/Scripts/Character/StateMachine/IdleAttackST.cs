using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class IdleAttackST : StateMachine
{
    protected SpecialForceController specialForce;
    protected SpecialForceAnimationController specialForceAnimation;
    public IdleAttackST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) : 
        base(agent, player, enemy, controller)
    {
        specialForce = (SpecialForceController)enemy;
        specialForceAnimation = (SpecialForceAnimationController)controller;
    }
    public override void EnterState()
    {
        //Debug.Log("Enter Idle Attack");
        agent.ResetPath();
        agent.isStopped = true;
        specialForceAnimation.SetIdleShootAnimation();
    }

    public override void ExitState()
    {
        agent.isStopped = false;
    }

    public override void UpdateState()
    {
        specialForce.GetComponent<SpecialForce>().GetBehavior<IBehavior>(BehaviorType.Attack).UpdateBehavior(player);
        float distance = Vector3.Distance(specialForce.transform.position, player.position);

        if (distance > specialForce.chaseDistance)
        {
            specialForce.ChangeState(new PatrolST(agent, player, specialForce, specialForceAnimation));
            return;
        }
        if (distance <= specialForce.chaseDistance && distance > specialForce.chaseAttackDistance)
        {
            specialForce.ChangeState(new ChaseST(agent, player, specialForce, specialForceAnimation));
            return;
        }
        if (distance <= specialForce.chaseAttackDistance && distance > specialForce.idleAttackDistance)
        {
            specialForce.ChangeState(new ChaseAttackST(agent, player, specialForce, specialForceAnimation));
            return;
        }
    }
}
