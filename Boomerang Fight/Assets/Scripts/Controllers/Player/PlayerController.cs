using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviourPun
{

    //[SerializeField] UserInputPhone _input;
    [Header("Objects")]
    [SerializeField] GameObject _playerBody;
    [SerializeField] GameObject _boomerangVisual;
    [Header("Components")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Collider _bodyCollider;
    [SerializeField] RangeAbility _rangeAbility;
    [SerializeField] RecallAbility _recallAbility;
    [SerializeField] DashAbility _dashAbility;
    [SerializeField] MeleeAbility _meleeAbility;
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] VFXTransitioner _vfxActivator;
    public VFXTransitioner VFXTransitioner { get { return _vfxActivator; } private set { _vfxActivator = value; } }
    [SerializeField] Boomerang _boomerang;
    [Header("JoySticks Set-UP")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [Header("Player Movement Stats")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _timeToAccelerate = 0.2f;
    [SerializeField] float _timeToDecelerate = 0.2f;
    [Header("Falling Parameters")]
    [SerializeField] float _groundDistanceCheck;
    [SerializeField] float _delayTillFallFromWalk;
    [SerializeField] float _delayTillFallFromDash;

    [Header("Actions")]
    public UnityEvent OnRecall;
    public Action OnChargeStart; //for activating indicators
    public Action OnCharging; //for updating indicator position
    public Action OnRelease; //for removing indicators

    private Action OnMasterPlayerControllerUpdate;
    private Action OnLocalPlayerControllerFixedUpdate;
    private Action OnLocalPlayerControllerUpdate;

    int _mySpawnIndex;
    float _currentSpeed = 0f;
    Vector3 _moveVelocity = Vector3.zero;
    Vector3 _attackDirection = Vector3.forward;
    float _fallTimer = 0f;

    [Header("State guards and triggers")]
    bool _canMove = true;
    bool _falling = false;
    bool _startedRangeAbility = false;


    float Acceleration => _moveSpeed / _timeToAccelerate;
    float Deceleration => _moveSpeed / _timeToDecelerate;
    float DelayTillFall => _dashAbility.InDash ? _delayTillFallFromDash : _delayTillFallFromWalk;

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

        //checks if its not my player.
        if (!photonView.IsMine)
        {
            //close other players joysticks canvas
            _joystickCanvas.SetActive(false);
        }
        else
        {
            gameObject.layer = 3;//player layer
            //set camera follow to my player
            CameraManager.Instance.CameraFollowRef.SetTarget(_playerBody.transform);
        }
    }
    private void OnEnable()
    {
        if (!photonView.IsMine)
            return;

        _canMove = true;
        //on my player, handle movement in update.
        SubscribeEvents();
    }
    private void OnDisable()
    {
        if (!photonView.IsMine)
            return;

        _canMove = false;
        //In order to prevent resource leaks, unsubscribe events
        UnsubscribeEvents();
    }
    private void Update()
    {
        OnLocalPlayerControllerUpdate?.Invoke();
    }
    private void FixedUpdate()
    {
        LocalPlayerControlUpdate();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 8)
            return;

        if (_dashAbility.InDash && 1 - _dashAbility.DashDuration.Progress < 0.7f)
        {
            _dashAbility.DashDuration.Stop();
        }
        print("dash disabled");
        _dashAbility.EnableDash(false);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != 8)
            return;

        print("dash enabled");
        _dashAbility.EnableDash(true);
    }

    private void SubscribeEvents()
    {
        OnLocalPlayerControllerFixedUpdate += HandleMovement;
        OnLocalPlayerControllerUpdate += HandleFalling;
        EnableRangeAbility();
        EnableRecallAbility();
        _boomerang.OnAttach += ToggleVisualBoomerang;
        _boomerang.OnRelease += ToggleVisualBoomerang;
        _boomerang.OnRelease += FaceThrowDirection;
        _meleeAbility.OnAttack += PlayerMelee;
        _dashAbility.OnDash += _playerAnimationController.DashPressedTrigger;
        _dashAbility.OnDash += _meleeAbility.DisableAttack;
        _dashAbility.OnDashEnd += _meleeAbility.EnableAttack;
    }
    private void UnsubscribeEvents()
    {
        OnLocalPlayerControllerFixedUpdate -= HandleMovement;
        OnLocalPlayerControllerUpdate -= HandleFalling;
        DisableRangeAbility();
        DisableRecallAbility();
        _boomerang.OnAttach -= ToggleVisualBoomerang;
        _boomerang.OnRelease -= ToggleVisualBoomerang;
        _boomerang.OnRelease -= FaceThrowDirection;
        _meleeAbility.OnAttack -= PlayerMelee;
        _dashAbility.OnDash -= _playerAnimationController.DashPressedTrigger;
        _dashAbility.OnDash -= _meleeAbility.DisableAttack;
        _dashAbility.OnDashEnd -= _meleeAbility.EnableAttack;
    }
    private void PlayerMelee()
    {
        _playerAnimationController.AttackPressedTrigger();
        _vfxActivator.ActivateVFX(VFXTypeEnum.Slap);
    }
    private void HandleMovement()
    {
        if (!_canMove)
            return;
        if (_dashAbility.InDash)
            return;
        if (_meleeAbility.InAttack)
            return;

        //get movement from joystick
        Vector3 inputDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        //check magnitude size
        if (inputDirection.magnitude > 0.1)
        {
            if (!_rangeAbility.Aimed)
            {
                _attackDirection = inputDirection;
                _rangeAbility.CalculateAttackDirection(_attackDirection);
            }

            //movement
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _moveSpeed, Acceleration * Time.deltaTime);
            _moveVelocity = inputDirection * _currentSpeed;

            //vfx
            _vfxActivator.ActivateVFX(VFXTypeEnum.Walking);

            //animations
            _playerAnimationController.StartWalk();
            _playerBody.transform.forward = inputDirection;

            //ability updates
            _dashAbility.UpdateDashDirection(inputDirection);
        }
        else
        {
            //vfx
            _vfxActivator.DeActivateProlongedVFX(VFXTypeEnum.Walking);
            //movement
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, Deceleration * Time.deltaTime);
            _moveVelocity = _moveVelocity.normalized * _currentSpeed;

            //animations
            _playerAnimationController.StopWalk();
        }
        _rb.velocity = _moveVelocity;
    }

    void HandleFalling()
    {
        if (!_canMove)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down* _groundDistanceCheck, out hit, _groundDistanceCheck))
        {
            _fallTimer = 0f;
        }
        else
        {
            _fallTimer += Time.deltaTime;
        }

        if (_fallTimer > DelayTillFall)
        {
            Fall();
        }
    }

    private void Fall()
    {
        print("player " + gameObject.name + "is falling");
        _falling = true;
        _canMove = false;
        _rb.velocity = Vector3.zero;
        _rb.constraints &= ~RigidbodyConstraints.FreezePositionY; // Remove Y position constraint
        _bodyCollider.isTrigger = true;
        //play falling animation
    }

    private void LocalPlayerControlUpdate()
    {
        if (photonView.IsMine)
            OnLocalPlayerControllerFixedUpdate?.Invoke();
    }

    #region Recall Ability
    private void HandleRecall()
    {
        if (!photonView.IsMine)
            return;
        //checks if can recall boomerang.
        if (_recallAbility.PlayerBoomerang.CanRecall())
        {
            _vfxActivator.ActivateVFX(VFXTypeEnum.Recall);
            //recalling boomerang.
            _recallAbility.UseAbility();
        }
    }
    private void RecallOff()
    {
        _recallAbility.PlayerBoomerang.StopRecall();
        _vfxActivator.DeActivateProlongedVFX(VFXTypeEnum.Recall);
    }
    private void EnableRecallAbility()
    {
        _AttackJoystick.OnJoystickPressed += HandleRecall;
        _AttackJoystick.OnJoystickUp += RecallOff;
    }
    private void DisableRecallAbility()
    {
        _AttackJoystick.OnJoystickPressed -= HandleRecall;
        _AttackJoystick.OnJoystickUp -= RecallOff;
    }
    #endregion Recall Ability

    #region Range Ability
    private void UseRangeAbility()
    {
        if (!photonView.IsMine)
            return;
        if (!_startedRangeAbility)
            return;

        print("used range ability (joystick up)");

        StopRangeAbility();
        FaceThrowDirection();
        _rangeAbility.UseAbility();
    }
    private void HandleRangeAbilityDirection()
    {
        if (!photonView.IsMine)
            return;
        if (!_startedRangeAbility) 
            return;

        Vector3 newAttackDirection = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;

        if (newAttackDirection.magnitude > 0.1f)
        {
            _attackDirection = newAttackDirection;
            _rangeAbility.Aimed = true;
            _rangeAbility.CalculateAttackDirection(_attackDirection);
        }
    }
    private void StopRangeAbility()
    {
        _startedRangeAbility = false;
        OnRelease?.Invoke();
        _playerAnimationController.StopChargingBoomerang();
        _vfxActivator.DeActivateProlongedVFX(VFXTypeEnum.Arrow, true);
    }

    private void StartRangeAbility()
    {
        if (!photonView.IsMine)
            return;

        //we need a state machine
        if (_meleeAbility.InAttack)
            return;
        if (_dashAbility.InDash)
            return;
        if (_falling)
            return;
        if (_boomerang.gameObject.activeInHierarchy)
            return;

        _startedRangeAbility = true;
        OnChargeStart?.Invoke();
        _playerAnimationController.StartChargingBoomerang();
        print("start charging boomerang");
        //range ability start charge timer
        _rangeAbility.StartCharge();
        _vfxActivator.ActivateVFX(VFXTypeEnum.Arrow, true);
    }
    private void EnableRangeAbility()
    {
        _AttackJoystick.OnJoystickDown += StartRangeAbility;
        _AttackJoystick.OnJoystickUp += UseRangeAbility;
        _AttackJoystick.OnJoystickDrag += HandleRangeAbilityDirection;
    }
    private void DisableRangeAbility()
    {
        _AttackJoystick.OnJoystickDown -= StartRangeAbility;
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
        _playerBody.transform.forward = _attackDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * _groundDistanceCheck);
    }

}
