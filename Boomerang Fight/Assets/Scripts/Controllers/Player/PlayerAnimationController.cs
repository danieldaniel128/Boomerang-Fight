using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    const string WALKING_BOOL = "Walking";
    const string DASH_PRESSED_TRIGGER = "DashPressed";
    const string CHARGING_BOOMERANG_BOOL = "ChargingBoomerang";
    const string ATTACK_PRESSED_TRIGGER = "AttackPressed";

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
        characterAnimator.SetBool(CHARGING_BOOMERANG_BOOL, true);
    }
    public void StopChargingBoomerang()
    {
        characterAnimator.SetBool(CHARGING_BOOMERANG_BOOL, false);
    }
    public void AttackPressedTrigger()
    {
        characterAnimator.SetTrigger(ATTACK_PRESSED_TRIGGER);
    }
    public void DashPressedTrigger()
    {
        characterAnimator.SetTrigger(DASH_PRESSED_TRIGGER);
    }
}
