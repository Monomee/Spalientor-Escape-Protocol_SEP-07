using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    BaseCharacter character;
    protected const string DETECTED_PLAYER = "detectedPlayer";
    protected const string MOVE = "isMoving";
    protected const string ALIVE = "isAlive";

    protected void Init()
    {
        character = GetComponent<BaseCharacter>();
        switch (character.characterType)
        {
            case CharacterType.Scientist: character = (Scientist)character; break;
            case CharacterType.SpecialForce: character = (SpecialForce)character; break;
        }
    }

    public void SetDeadAnimation()
    {
        animator.SetBool(ALIVE, false);
        return;
    }
    public void ResetEnemy()
    {
        animator.SetBool(ALIVE, true);
    }

}
