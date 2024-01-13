using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[CreateAssetMenu(fileName = "OldInput", menuName = "InputSystem/Old Input", order = 1)]
public class OldInputSystem : UserInputPhone
{
    //GetTouchPhase
    //GetTouchPhase
    public override void SubscribeInputActions()
    {
        base.SubscribeInputActions();
    }

    public override void UnsubscribeInputActions()
    {
        base .UnsubscribeInputActions();

    }
    public override Vector2 GetTouchPosition()
    {
        return Vector2.zero;
    }
    public override TouchPhase GetTouchPhase()
    {
        return TouchPhase.None;
    }

}
