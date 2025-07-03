using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Player : BaseCharacter
{
    public List<GameObject> inventory;
    public float stamina = 100; 
    public bool isStealth;
    [HideInInspector]public Attack attack;
    [HideInInspector] public IBehavior behavior;
    [SerializeField] float defense;
    [SerializeField] float health;
    public void SetupPlayer()
    {
        AddBehavior(BehaviorType.Attack, new Attack());
        AddBehavior(BehaviorType.Health, new HealthStatus(defense, health));
    }
    private void Start()
    {
        SetupPlayer();
        Init();
        attack = GetBehavior<Attack>(BehaviorType.Attack);       
    }
    private void Update()
    {
        if (attack.CurrentGun == null)
        {
            attack.Initialize(this);
        }
        if (attack.CurrentGun != null) attack.UpdateBehavior();
    }
}
