using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistController : BaseEnemyController
{
    public float escapeDistance = 20f;
    // Start is called before the first frame update
    void Start()
    {
        SetPlayerTransform();
        currentState = new IdleST(agent, player, this, this.GetComponent<ScientistAnimationController>());
    }


}
