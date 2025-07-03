using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolST : StateMachine
{
    protected SpecialForceController specialForce;
    protected ScientistController scientist;
    protected SpecialForceAnimationController specialForceAnimation;
    protected ScientistAnimationController scientistAnimation;

    private float idleDuration = 30f;
    private float timer = 0f;
    public PatrolST(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller) : 
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
        //Debug.Log("Enter Patrol");
        timer = 0f;
        if (specialForce != null)
        {
            specialForceAnimation.gunPos = specialForce.GetComponent<SpecialForce>().GetBehavior<Attack>(BehaviorType.Attack).CurrentGun.transform;

            specialForceAnimation.SetPatrolAnimation();

        }
        else if (scientist != null)
        {
            scientistAnimation.SetWalkAnimation();
        }
        GoToNextRandomPoint();
    }

    public override void ExitState()
    {
        agent.ResetPath();
    }

    public override void UpdateState()
    {
        if (agent.pathPending)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();
        float actionDistance = GetActionDistance();

        if (specialForce != null)
        {
            if (distanceToPlayer <= specialForce.chaseDistance)
            {
                if (canSeePlayer)
                {
                    enemy.ChangeState(new ChaseST(agent, player, enemy, specialForceAnimation));
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
        timer += Time.deltaTime;
        if (agent.remainingDistance < 0.5f || timer >= idleDuration)
        {
            enemy.ChangeState(new IdleST(agent, player, enemy, controller));
            return;
        }
    }

    public float range = 10f;  
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    void GoToNextRandomPoint()
    {
        //Vector3 point;
        //if (RandomPoint(Vector3.zero, range, out point))
        //{
        //    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        //    agent.SetDestination(point);
        //}
        //else
        //{
        //    //Debug.Log("Nothing");
        //}
        agent.SetDestination(GameManager.Instance.GetNextPoint());
    }
    
}
