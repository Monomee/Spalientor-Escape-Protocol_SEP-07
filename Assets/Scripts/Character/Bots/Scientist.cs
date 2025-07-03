using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : BaseCharacter
{
    public bool detectedPlayer;
    [SerializeField] float defense;
    [SerializeField] float health;
    [SerializeField] float speed;
    public void SetupScientist() 
    {
        AddBehavior(BehaviorType.Health, new HealthStatus(defense, health));
        AddBehavior(BehaviorType.Move, new Move(speed));
    }
    private void Start()
    {
        SetupScientist();
        Init();
    }
    private void Update()
    {
        if (!isDead)
        {
            CheckDead();
        }
    }
}
