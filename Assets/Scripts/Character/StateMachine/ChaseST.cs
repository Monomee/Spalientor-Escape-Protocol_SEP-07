using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ChaseST : StateMachine
{
    protected SpecialForceController specialForce;
    protected ScientistController scientist;
    protected SpecialForceAnimationController specialForceAnimation;
    protected ScientistAnimationController scientistAnimation;
    public ChaseST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) :
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
            specialForceAnimation = (SpecialForceAnimationController)controller;
        }
        else
        {
            Debug.LogError($"Unsupported enemy type: {enemy.GetType().Name}");
        }
    }

    float distance;
    public override void EnterState()
    {
        //Debug.Log("Enter Chase");
        specialForceAnimation.SetChaseAnimation();
    }

    public override void ExitState()
    {
        agent.ResetPath();
    }

    public override void UpdateState()
    {
        agent.SetDestination(player.position);

        distance = Vector3.Distance(specialForce.transform.position, player.position);

        if (distance > specialForce.chaseDistance)
        {
            specialForce.ChangeState(new PatrolST(agent, player, specialForce, specialForceAnimation));
            return;
        }      

        if (distance <= specialForce.chaseAttackDistance && distance > specialForce.idleAttackDistance)
        {           
            specialForce.ChangeState(new ChaseAttackST(agent, player, specialForce, specialForceAnimation));
            return;
        }

        if (distance <= specialForce.idleAttackDistance)
        {
            specialForce.ChangeState(new IdleAttackST(agent, player, specialForce, specialForceAnimation));
            return;
        }
    }


}
