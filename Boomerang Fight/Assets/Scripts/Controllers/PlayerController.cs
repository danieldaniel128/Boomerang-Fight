using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{

    //[SerializeField] UserInputPhone _input;
    [SerializeField] GameObject _playerBody;
    [Header("Components")]
    [SerializeField] CameraFollow _cameraFollow;
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
        //set camera reference
        _cameraFollow = Camera.main.GetComponent<CameraFollow>();
        //checks if its not my player.
        if (!photonView.IsMine)
        {
            //close other players joysticks canvas
            _joystickCanvas.SetActive(false);
        }
        else//set camera follow to my player
            _cameraFollow.target = _playerBody.transform;

    }
    private void OnEnable()
    {
        //on my player, handle movement in update.
        OnLocalPlayerControllerUpdate += HandleMovement;
        //when press attack button, enable attack.
        _AttackJoystick.OnJoystickDown += EnableRangeAttack;
        //when is pressed, handle recall boomerang.
        _AttackJoystick.OnJoystickPressed += HandleRecall;
    }
    private void OnDisable()
    {
        //In order to prevent resource leaks, unsubscribe events
        OnLocalPlayerControllerUpdate -= HandleMovement;
        _AttackJoystick.OnJoystickPressed -= HandleRecall;
        _AttackJoystick.OnJoystickDown -= EnableRangeAttack;
        DisableRangeAttack();
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
    private void HandleRecall()
    {
        if (!photonView.IsMine)
            return;
        //checks if can recall boomerang.
        if (_battlerangAttack.CanRecall())
        {
            //when recalling, disable attack
            DisableRangeAttack();
            //recalling boomerang.
            _battlerangAttack.Recall();
        }
    }
    private void HandleAttack()
    {
        if (!photonView.IsMine)
            return;
        if (_AttackJoystick.Direction != Vector2.zero)
        {
            Vector3 attackDirection = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;
            _battlerangAttack.AttackRange(attackDirection);
        }
        //else auto aim like brawl stars?
    }
    private void EnableRangeAttack()
    {
        _AttackJoystick.OnJoystickUp += HandleAttack;
    }
    private void DisableRangeAttack()
    {
        _AttackJoystick.OnJoystickUp -= HandleAttack;
    }

}
