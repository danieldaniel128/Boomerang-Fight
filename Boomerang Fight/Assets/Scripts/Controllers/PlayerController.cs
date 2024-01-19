using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{

    //[SerializeField] UserInputPhone _input;
    [Header("JoySticks")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [SerializeField] float moveSpeed;
    Action OnMasterPlayerControllerUpdate;
    Action OnLocalPlayerControllerUpdate;

    private void OnEnable()
    {
        OnLocalPlayerControllerUpdate += HandleMovement;
    }
    private void OnDisable()
    {
        OnLocalPlayerControllerUpdate -= HandleMovement;
    }

    private void Update()
    {
        IsLocalPlayer();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        transform.position +=  moveDirection * moveSpeed * Time.deltaTime;
        //do logic
    }
    private void IsLocalPlayer()
    {
        if(photonView.IsMine)
            OnLocalPlayerControllerUpdate?.Invoke();
        else
        {
            _joystickCanvas.SetActive(false);
        }
    }
}
