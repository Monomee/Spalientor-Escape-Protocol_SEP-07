using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscapeST : StateMachine
{
    protected ScientistController scientist;
    protected ScientistAnimationController scientistAnimation;
    public EscapeST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) :
        base(agent, player, enemy, controller)
    {
        scientist = (ScientistController)enemy;
        scientistAnimation = (ScientistAnimationController)controller;
    }

    public override void EnterState()
    {
        scientistAnimation.SetEscapeAnimation();
    }

    public override void ExitState()
    {
        agent.ResetPath();
    }

    public override void UpdateState()
    {
        agent.SetDestination(Vector3.zero);
        if (Vector3.Distance(agent.transform.position, Vector3.zero) < 0.5f)
        {
            enemy.ChangeState(new IdleST(agent, player, enemy, controller));
            return;
        }
    }
}
