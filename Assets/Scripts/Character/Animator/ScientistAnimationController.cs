using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistAnimationController : AnimationController
{
    public void SetWalkAnimation()
    {
        animator.SetBool(MOVE, true);
        animator.SetBool(DETECTED_PLAYER, false);
    }
    public void SetIdleAnimation()
    {
        animator.SetBool(MOVE, false);
        animator.SetBool(DETECTED_PLAYER, false);
    }
    public void SetEscapeAnimation()
    {
        animator.SetBool(DETECTED_PLAYER, true);
    }
}
