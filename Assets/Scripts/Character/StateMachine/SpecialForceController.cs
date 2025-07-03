using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForceController : BaseEnemyController
{
    public float chaseDistance;
    public float idleAttackDistance;
    public float chaseAttackDistance;

    void Start()
    {
        SetPlayerTransform();
        currentState = new IdleST(agent, player, this, this.GetComponent<AnimationController>());
    }
}
