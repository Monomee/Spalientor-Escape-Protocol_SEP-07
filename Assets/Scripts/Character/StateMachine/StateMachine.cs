using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class StateMachine : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
    protected BaseEnemyController enemy;
    protected AnimationController controller;
    protected StateMachine(NavMeshAgent agent, Transform player, BaseEnemyController enemy, AnimationController controller)
    {
        this.agent = agent;
        this.player = player;
        this.enemy = enemy;
        this.controller = controller;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();

    public bool CanSeePlayer()
    {
        // Field of view parameters
        float viewAngle = 45f; // Góc nh?n (ð?)
        int rayCount = 7;      // S? lý?ng ray trong h?nh nón
        float rayHeight = 1f; // Ð? cao ray (t?m m?t)

        Vector3 eyePos = enemy.transform.position + Vector3.up * rayHeight;
        Vector3 toPlayer = (player.position + Vector3.up * rayHeight - eyePos).normalized;
        float angleToPlayer = Vector3.Angle(enemy.transform.forward, toPlayer);

        if (angleToPlayer <= viewAngle / 2f)
        {
            // B?n nhi?u ray trong h?nh nón
            for (int i = 0; i < rayCount; i++)
            {
                // Tính hý?ng l?ch cho t?ng ray
                float lerp = (rayCount == 1) ? 0.5f : (float)i / (rayCount - 1);
                float angleOffset = Mathf.Lerp(-viewAngle / 2f, viewAngle / 2f, lerp);
                Vector3 rayDir = Quaternion.Euler(0, angleOffset, 0) * toPlayer;

                RaycastHit hit;
                float distance = GetActionDistance();

                if (Physics.Raycast(eyePos, rayDir, out hit, distance, LayerMask.GetMask("Default", "canBeAttacked")))
                {
                    Debug.DrawRay(eyePos, rayDir * distance, Color.red, 0.1f);
                    if (hit.transform.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public float GetActionDistance()
    {
        if (enemy is SpecialForceController specialForce)
        {
            return specialForce.chaseDistance;
        }
        else if (enemy is ScientistController scientist)
        {
            return scientist.escapeDistance;
        }
        return 0;
    }
} 
