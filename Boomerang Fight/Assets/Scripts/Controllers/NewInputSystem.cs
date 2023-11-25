using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static InputAsset;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[CreateAssetMenu(fileName = "NewInput", menuName = "InputSystem/New Input", order = 1)]
public class NewInputSystem : UserInputPhone, IPlayerActions
{
    InputAsset _input;

    public void SubscribeInputActions()//subscribe on enable
    {
        _input = new InputAsset();
        _input.Player.SetCallbacks(this);
    }
    public void UnsubscribeInputActions()//unsubsribed on disable
    {
        _input.Player.RemoveCallbacks(this);
    }

    public void OnTouchPhase(InputAction.CallbackContext context)
    {
        this.TouchPhase = context.ReadValue<TouchPhase>();
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        TouchPosition = context.ReadValue<Vector2>();
    }
}

