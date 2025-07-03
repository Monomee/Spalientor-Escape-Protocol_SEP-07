using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyController : MonoBehaviour
{
    protected StateMachine currentState;
    public Transform player;
    public NavMeshAgent agent;

    protected void SetPlayerTransform()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState();
    }
    public void ChangeState(StateMachine newState)
    {
        currentState.ExitState();

        currentState = newState;
        currentState.EnterState();
    }
    public void ResetState()
    {
        if (currentState is DeadST)
        {
            GetComponent<AnimationController>().ResetEnemy();
            currentState = new IdleST(agent, player, this, GetComponent<AnimationController>());
            currentState.EnterState();
        }
        return;
    }
    public void DeadState()
    {
        ChangeState(new DeadST(agent, player, this, GetComponent<AnimationController>()));
    }
}
