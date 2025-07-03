using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForceAnimationController : AnimationController
{
    public const string SHOOT = "isShooting";
    public const string RELOAD_TRIGGER = "isReloading";
    public Transform idleHoldGunPos;
    public Transform idleShootHoldGunPos;
    public Transform chaseShootHoldGunPos;
    public Transform gunPos;

    public void SetIdleAnimation()
    {
        SetGunTransform(idleHoldGunPos);
        animator.SetBool(MOVE, false);
        animator.SetBool(DETECTED_PLAYER, false);
        animator.SetBool(SHOOT, false);
    }   
    public void SetPatrolAnimation()
    {
        SetGunTransform(idleHoldGunPos);
        animator.SetBool(MOVE, true);
        animator.SetBool(DETECTED_PLAYER, false);
    }
    public void SetChaseAnimation()
    {
        animator.SetBool(MOVE, true);
        animator.SetBool(DETECTED_PLAYER, true);
    }
    public void SetIdleShootAnimation()
    {
        SetGunTransform(idleShootHoldGunPos);
        animator.SetBool(MOVE, false);
        animator.SetBool(DETECTED_PLAYER, true);
        animator.SetBool(SHOOT, true);
    }
    public void SetChaseShootAnimation()
    {
        SetGunTransform(chaseShootHoldGunPos);
        animator.SetBool(SHOOT, true);
        animator.SetBool(DETECTED_PLAYER, true);
    }
    public void SetReload()
    {
        animator.SetTrigger(RELOAD_TRIGGER);
    }

    public void SetGunTransform(Transform newTransform)
    {
        gunPos.SetParent(newTransform);
        gunPos.localPosition = Vector3.zero;
        gunPos.localRotation = Quaternion.identity;
    }
}
