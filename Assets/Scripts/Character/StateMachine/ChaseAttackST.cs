using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAttackST : StateMachine
{
    protected SpecialForceController specialForce;
    protected SpecialForceAnimationController specialForceAnimation;
    public ChaseAttackST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) :
        base(agent, player, enemy, controller)
    {
        specialForce = (SpecialForceController)enemy;
        specialForceAnimation = (SpecialForceAnimationController)controller;
    }
    public override void EnterState()
    {
        //Debug.Log("Enter Chase Attack");
        agent.ResetPath();
        specialForceAnimation.SetChaseShootAnimation();
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        specialForce.GetComponent<SpecialForce>().GetBehavior<IBehavior>(BehaviorType.Attack).UpdateBehavior(player);
        agent.SetDestination(player.position);

        float distance = Vector3.Distance(specialForce.transform.position, player.position);

        if (distance <= specialForce.idleAttackDistance)
        {
            specialForce.ChangeState(new IdleAttackST(agent, player, specialForce, specialForceAnimation));
            return;
        }

        if (distance <= specialForce.chaseDistance && distance > specialForce.chaseAttackDistance)
        {
            specialForce.ChangeState(new ChaseST(agent, player, specialForce, specialForceAnimation));
            return;
        }

        if (distance > specialForce.chaseDistance)
        {
            specialForce.ChangeState(new PatrolST(agent, player, specialForce, specialForceAnimation));
            return;
        }
    }
}
