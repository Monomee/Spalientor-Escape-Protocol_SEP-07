
using UnityEngine;

public interface IBehavior 
{
    public void Initialize(BaseCharacter owner);
    public void UpdateBehavior(Transform target);
}
