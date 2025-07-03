
using UnityEngine;
using UnityEngine.AI;

public class Move : IBehavior
{
    private BaseCharacter owner;
    private NavMeshAgent agent;
    private float speed;

    public Move(float speed)
    {
        Speed = speed;
    }

    public float Speed { get => speed; set => speed = value; }

    public void Initialize(BaseCharacter owner)
    {
        this.owner = owner;
        agent = owner.GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void UpdateBehavior(Transform target)
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }
}
