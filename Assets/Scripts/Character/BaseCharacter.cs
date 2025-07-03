using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterType characterType;
    public bool isAlive = true;
    public bool isDead = false;
    public Dictionary<BehaviorType, IBehavior> behaviorDictionary = new Dictionary<BehaviorType, IBehavior>();
    // Start is called before the first frame update
    protected void Init()
    {
        foreach (KeyValuePair<BehaviorType, IBehavior> pair in behaviorDictionary)
        {
            pair.Value.Initialize(this);
        }
    }

    // Update is called once per frame
    protected void CheckDead()
    {
        if (!isAlive)
        {
            GetComponent<BaseEnemyController>().DeadState();     
            isDead = true;
        }
    }
    public T GetBehavior<T>(BehaviorType type) where T : class, IBehavior
    {
        if (behaviorDictionary.TryGetValue(type, out var behavior))
            return behavior as T;
        return null;
    }
    public void AddBehavior(BehaviorType type, IBehavior behavior)
    {
        if (!behaviorDictionary.ContainsKey(type))
            behaviorDictionary.Add(type, behavior);
    }
}
public enum CharacterType
{
    Player,
    Scientist,
    SpecialForce
}
public enum BehaviorType
{
    Health,
    Attack,
    Move
}
