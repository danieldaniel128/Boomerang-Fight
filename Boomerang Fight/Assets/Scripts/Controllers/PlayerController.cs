using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviourPun
{

    //[SerializeField] UserInputPhone _input;
    [SerializeField] GameObject _playerBody;
    [SerializeField] GameObject _boomerangVisual;
    [Header("Components")]
    [SerializeField] CameraFollow _cameraFollow;
    [SerializeField] RangeAbility _rangeAbility;
    [SerializeField] RecallAbility _recallAbility;
    [SerializeField] DashAbility _dashAbility;
    [SerializeField] MeleeAbility _meleeAbility;
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] Boomerang _boomerang;
    [Header("JoySticks Set-UP")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [Header("Player Stats")]
    [SerializeField] float _moveSpeed;
    [Header("Actions")]
    public UnityEvent OnRecall;

    private Action OnMasterPlayerControllerUpdate;
    private Action OnLocalPlayerControllerUpdate;
    int _mySpawnIndex;
    [PunRPC]
    void SetMyPlayerIndex(int index)
    {
        _mySpawnIndex = index;
    }
    public int GetPlayerIndex()
    {
        return _mySpawnIndex;
    }
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
        else
        {
            //set camera follow to my player
            _cameraFollow.target = _playerBody.transform;
        }
        Debug.Log("OnStart");
    }
    private void OnEnable()
    {
        //on my player, handle movement in update.
        Subscribe();
    }

    

    private void OnDisable()
    {
        //In order to prevent resource leaks, unsubscribe events
        UnsubscribeEvents();
    }



    private void Update()
    {
        LocalPlayerControlUpdate();
    }
    private void Subscribe()
    {
        OnLocalPlayerControllerUpdate += HandleMovement;
        //when press attack button, enable attack.
        _AttackJoystick.OnJoystickDown += EnableRangeAbility;
        _AttackJoystick.OnJoystickDown += EnableRecallAbility;
        _boomerang.OnAttach += ToggleVisualBoomerang;
        _boomerang.OnRelease += ToggleVisualBoomerang;
        _boomerang.OnRelease += FaceThrowDirection;
        _meleeAbility.OnAttack += _playerAnimationController.AttackPressedTrigger;
        _dashAbility.OnDash += _playerAnimationController.DashPressedTrigger;
    }
    private void UnsubscribeEvents()
    {
        OnLocalPlayerControllerUpdate -= HandleMovement;
        _AttackJoystick.OnJoystickDown -= EnableRangeAbility;
        _AttackJoystick.OnJoystickDown -= EnableRecallAbility;
        DisableRangeAbility();
        DisableRecallAbility();
        _boomerang.OnAttach -= ToggleVisualBoomerang;
        _boomerang.OnRelease -= ToggleVisualBoomerang;
        _boomerang.OnRelease -= FaceThrowDirection;
        _meleeAbility.OnAttack -= _playerAnimationController.AttackPressedTrigger;
        _dashAbility.OnDash -= _playerAnimationController.DashPressedTrigger;
    }
    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        if (moveDirection.magnitude > 0)
        {
            _playerAnimationController.StartWalk();
            _playerBody.transform.forward = moveDirection;
        }
        else
        {
            _playerAnimationController.StopWalk();
        }
    }
    private void LocalPlayerControlUpdate()
    {
        if (photonView.IsMine)
            OnLocalPlayerControllerUpdate?.Invoke();
    }

    #region Recall Ability
    private void HandleRecall()
    {
        if (!photonView.IsMine)
            return;
        //checks if can recall boomerang.
        if (_recallAbility.PlayerBoomerang.CanRecall())
        {
            //when recalling, disable attack
            DisableRangeAbility();
            //recalling boomerang.
            _recallAbility.UseAbility();
        }
    }
    private void EnableRecallAbility()
    {
        _AttackJoystick.OnJoystickPressed += HandleRecall;
        _AttackJoystick.OnJoystickUp += _recallAbility.PlayerBoomerang.StopRecall;
    }
    private void DisableRecallAbility()
    {
        _AttackJoystick.OnJoystickPressed -= HandleRecall;
        _AttackJoystick.OnJoystickUp -= _recallAbility.PlayerBoomerang.StopRecall;
    }
    #endregion Recall Ability

    #region Range Ability
    private void UseRangeAbility()
    {
        if (!photonView.IsMine)
            return;
        StopCharge();
        _rangeAbility.UseAbility();
    }
    private void HandleRangeAbilityDirection()
    {
        if (!photonView.IsMine)
            return;
        Vector3 attackDirection = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;
        _rangeAbility.CalculateAttackRange(attackDirection);
    }
    private void StartCharge()
    {
        if (!photonView.IsMine)
            return;
        _playerAnimationController.StartChargingBoomerang();
        //range ability start charge timer
    }
    private void StopCharge()
    {
        if (!photonView.IsMine)
            return;
        _playerAnimationController.StopChargingBoomerang();
    }

    private void EnableRangeAbility()
    {
        _AttackJoystick.OnJoystickDown += StartCharge;
        _AttackJoystick.OnJoystickUp += UseRangeAbility;
        _AttackJoystick.OnJoystickDrag += HandleRangeAbilityDirection;
    }
    private void DisableRangeAbility()
    {
        _AttackJoystick.OnJoystickDown -= StartCharge;
        _AttackJoystick.OnJoystickUp -= UseRangeAbility;
        _AttackJoystick.OnJoystickDrag -= HandleRangeAbilityDirection;
    }
    #endregion Range Ability

    void ToggleVisualBoomerang()
    {
        _boomerangVisual.SetActive(!_boomerangVisual.activeInHierarchy);
    }

    void FaceThrowDirection()
    {
        _playerBody.transform.forward = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;
    }
}
