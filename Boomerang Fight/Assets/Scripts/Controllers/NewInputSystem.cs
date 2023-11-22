using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputAsset;

[CreateAssetMenu(fileName = "NewInput", menuName = "InputSystem/New Input", order = 1)]
public class NewInputSystem : UserInputPhone, IPlayerActions
{
    //GetTouchPhase {}
    public void OnTouch(InputAction.CallbackContext context)
    {
        
    }
}
