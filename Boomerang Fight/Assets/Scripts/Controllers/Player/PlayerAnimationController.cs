using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    const string MOVEMENT_LAYER = "Movement Layer";
    const string AIMING_LAYER = "Aiming Layer";

    const string WALKING_BOOL = "Walking";
    const string DASH_PRESSED_TRIGGER = "DashPressed";
    const string DASH_HIT_WALL_TRIGGER = "DashHitWall";
    const string CHARGING_BOOMERANG_BOOL = "ChargingBoomerang";
    const string ATTACK_PRESSED_TRIGGER = "AttackPressed";
    const string FALLING_TRIGGER = "Falling";

    [SerializeField] Animator characterAnimator;

    public void StartWalk()
    {
        characterAnimator.SetBool(WALKING_BOOL, true);
    }
    public void StopWalk()
    {
        characterAnimator.SetBool(WALKING_BOOL, false);
    }
    public void StartChargingBoomerang()
    {
        characterAnimator.SetLayerWeight(characterAnimator.GetLayerIndex(AIMING_LAYER), 1);
        characterAnimator.SetBool(CHARGING_BOOMERANG_BOOL, true);
    }
    public void StopChargingBoomerang()
    {
        characterAnimator.SetBool(CHARGING_BOOMERANG_BOOL, false);
    }
    public void ResetAimingLayerWeight()
    {
        characterAnimator.SetLayerWeight(characterAnimator.GetLayerIndex(AIMING_LAYER), 0);
    }
    public void AttackPressedTrigger()
    {
        characterAnimator.SetTrigger(ATTACK_PRESSED_TRIGGER);
    }
    public void DashPressedTrigger()
    {
        characterAnimator.SetTrigger(DASH_PRESSED_TRIGGER);
    }
    public void DashHitWallTrigger()
    {
        characterAnimator.SetTrigger(DASH_HIT_WALL_TRIGGER);
    }
    public void FallingTrigger()
    {
        characterAnimator.SetBool(FALLING_TRIGGER, true);
    }

    public void ResetAnimations()
    {
        characterAnimator.Play("Idle");
    }

}
