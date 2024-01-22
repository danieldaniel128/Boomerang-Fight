using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{

    //[SerializeField] UserInputPhone _input;
    [SerializeField] GameObject _playerBody;
    [Header("Components")]
    [SerializeField] CameraFollow _camera;
    [SerializeField] BoomerangRangeAttack _battlerangAttack;

    [Header("JoySticks Set-UP")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [Header("Player Stats")]
    [SerializeField] float _moveSpeed;

    private Action OnMasterPlayerControllerUpdate;
    private Action OnLocalPlayerControllerUpdate;

    private void Start()
    {
        _camera = Camera.main.GetComponent<CameraFollow>();
        if (!photonView.IsMine)
        {
            _joystickCanvas.SetActive(false);
        }
        else
            _camera.target = _playerBody.transform;

    }
    private void OnEnable()
    {
        OnLocalPlayerControllerUpdate += HandleMovement;
        _AttackJoystick.OnJoystickUp += HandleAttack;
        _AttackJoystick.OnJoystickPressed += HandleRecall;
    }
    private void OnDisable()
    {
        OnLocalPlayerControllerUpdate -= HandleMovement;
        _AttackJoystick.OnJoystickUp -= HandleAttack;
        _AttackJoystick.OnJoystickPressed -= HandleRecall;
    }

    private void Update()
    {
        LocalPlayerControlUpdate();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        transform.position +=  moveDirection * _moveSpeed * Time.deltaTime;
        //do logic
    }
    private void LocalPlayerControlUpdate()
    {
        if(photonView.IsMine)
            OnLocalPlayerControllerUpdate?.Invoke();
    }
    private void HandleAttack()
    {
        if (_AttackJoystick.Direction != Vector2.zero)
        {
            Vector3 attackDirection = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;
            _battlerangAttack.AttackRange(attackDirection);
        }
        //else auto aim like brawl stars?
    }
    private void HandleRecall()
    {
        _battlerangAttack.Recall();
    }
}
