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
    public override void SubscribeInputActions()//subscribe on enable
    {
        _input = new InputAsset();
        _input.Player.TouchPhase.started += OnTouchPhase;
        _input.Player.TouchPhase.performed += OnTouchPhase;
        _input.Player.TouchPhase.canceled += OnTouchPhase;
        _input.Player.TouchPosition.started += OnTouchPosition;
        _input.Player.TouchPosition.performed += OnTouchPosition;
        _input.Player.TouchPosition.canceled += OnTouchPosition;
    }
    public override void UnsubscribeInputActions()//unsubsribed on disable
    {
        _input.Player.TouchPhase.started -= OnTouchPhase;
        _input.Player.TouchPhase.performed -= OnTouchPhase;
        _input.Player.TouchPhase.canceled -= OnTouchPhase;
        _input.Player.TouchPosition.started -= OnTouchPosition;
        _input.Player.TouchPosition.performed -= OnTouchPosition;
        _input.Player.TouchPosition.canceled -= OnTouchPosition;
    }

    public void OnTouchPhase(InputAction.CallbackContext context)
    {
        this.TouchPhase = context.ReadValue<TouchPhase>();
        Debug.Log($"touched phase {TouchPhase}");
    }

    public void OnTouchPosition(InputAction.CallbackContext context)
    {
        TouchPosition = context.ReadValue<Vector2>();
        Debug.Log($"touched phase {TouchPosition}");
    }
}

