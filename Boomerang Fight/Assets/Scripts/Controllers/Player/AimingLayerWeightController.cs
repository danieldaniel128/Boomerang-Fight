using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingLayerWeightController : MonoBehaviour
{
    [SerializeField] PlayerAnimationController playerAnimationController;
    public void ResetAimingLayerWeight()
    {
        print("reset weight to 0");
        playerAnimationController.ResetAimingLayerWeight();
    }
}
