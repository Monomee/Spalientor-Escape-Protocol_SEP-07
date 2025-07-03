
using UnityEngine;

public class HealthStatus : IBehavior
{
    private BaseCharacter owner;
    private float health;
    private float defense; //Reduces %

    public HealthStatus(float defense, float health)
    {
        Defense = defense;
        Health = health;
    }

    public float Defense { get => defense; set => defense = value; }
    public float Health { get => health; set => health = value; }

    public void Initialize(BaseCharacter owner)
    {
        this.owner = owner;
    }

    public void UpdateBehavior(Transform target)
    {

    }
    public bool TakeDamage(float damage)
    {       
        health -= (damage*(1-defense/100));
        UIManagerMap2.Instance.healthBar.value = health;
        return health > 0;
    }
    public void Heal(float healthToHeal, bool canHeal = false)
    {
        if (canHeal) //&& isAlive
        {
            health += healthToHeal;
        }
    }   
}
