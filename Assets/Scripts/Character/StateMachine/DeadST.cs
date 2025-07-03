using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadST : StateMachine
{
    private float duration = 5f;
    private float timer = 0f;
    public DeadST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) : base(agent, player, enemy, controller)
    {
    }

    public override void EnterState()
    {
        agent.ResetPath();
        timer = 0f;
        controller.SetDeadAnimation();
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            enemy.gameObject.SetActive(false);
            return;
        }
    }
}
