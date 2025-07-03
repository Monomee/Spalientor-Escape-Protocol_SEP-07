using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForce : BaseCharacter
{
    public bool detectedPlayer;

    [SerializeField] float defense;
    [SerializeField] float health;
    [SerializeField] float speed;
    public void SetupSpecialForce()
    {
        AddBehavior(BehaviorType.Health, new HealthStatus(defense, health));
        AddBehavior(BehaviorType.Move, new Move(speed));
        AddBehavior(BehaviorType.Attack, new Attack());
    }
    private void Start()
    {
        SetupSpecialForce();
        Init();
        
        Attack attack = GetBehavior<Attack>(BehaviorType.Attack);
        Rigidbody rb = attack.CurrentGun.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
    }

    private void Update()
    {
        if (!isDead)
        {
            CheckDead();          
        }
    }
}
