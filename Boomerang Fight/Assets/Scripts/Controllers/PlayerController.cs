using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    
    //[SerializeField] UserInputPhone _input;
    [Header("JoySticks")]
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [SerializeField] float moveSpeed;
    Action OnMasterPlayerController;
    Action OnLocalPlayerController;

    private void OnEnable()
    {
        //_input.SubscribeInputActions();
    }
    private void OnDisable()
    {
        //_input.UnsubscribeInputActions();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        transform.position +=  moveDirection * moveSpeed * Time.deltaTime;
        //do logic
    }
}
