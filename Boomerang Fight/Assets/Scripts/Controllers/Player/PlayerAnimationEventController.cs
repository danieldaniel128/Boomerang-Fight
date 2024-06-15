using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerAnimationController playerAnimationController;
    public void ResetAimingLayerWeight()
    {
        print("reset weight to 0");
        playerAnimationController.ResetAimingLayerWeight();
    }

    public void EndOfFallAnimation()
    {
        playerController.OnFallEnded?.Invoke();
    }
}
