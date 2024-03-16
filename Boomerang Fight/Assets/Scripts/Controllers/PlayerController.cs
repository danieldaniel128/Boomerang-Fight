using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] CameraFollow _cameraFollow;
    [SerializeField] RangeAbility _rangeAbility;
    [SerializeField] RecallAbility _recallAbility;
    [SerializeField] DashAbility _dashAbility;
    [SerializeField] MeleeAbility _meleeAbility;
    [SerializeField] PlayerAnimationController _playerAnimationController;
    [SerializeField] Boomerang _boomerang;
    [SerializeField] Collider _bodyCollider;
    [Header("JoySticks Set-UP")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [Header("Player Movement Stats")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _timeToAccelerate = 0.2f;
    [SerializeField] float _timeToDecelerate = 0.2f;
    [SerializeField] float _maxAngle = 80f;
    [SerializeField] float _distanceToWallCheck = 2f;
    [Header("Actions")]
    public UnityEvent OnRecall;

    private Action OnMasterPlayerControllerUpdate;
    private Action OnLocalPlayerControllerUpdate;
    int _mySpawnIndex;
    float _currentSpeed = 0f;
    Vector3 _moveDirection = Vector3.zero;

    float Acceleration => _moveSpeed / _timeToAccelerate;
    float Deceleration => _moveSpeed / _timeToDecelerate;


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
            gameObject.layer = 3;//player layer
            //set camera follow to my player
            _cameraFollow.target = _playerBody.transform;
        }
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
        //get movement from joystick
        Vector3 inputDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        //check magnitude size
        if (inputDirection.magnitude > 0)
        {
            //movement
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _moveSpeed, Acceleration * Time.deltaTime);
            _moveDirection = inputDirection * _currentSpeed;

            //animations
            _playerAnimationController.StartWalk();
            _playerBody.transform.forward = inputDirection;

            //ability updates
            _dashAbility.UpdateDashDirection(inputDirection);
        }
        else
        {
            //movement
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, Deceleration * Time.deltaTime);
            _moveDirection = _moveDirection.normalized * _currentSpeed;

            //animations
            _playerAnimationController.StopWalk();
        }

        //cast ray forward to check if wall/can move in that direction
        //check if can move forward
        Vector3 newPosition = AdjustMovementIfWall(_moveDirection * Time.deltaTime);
        transform.position += newPosition;
    }

    private Vector3 AdjustMovementIfWall(Vector3 MovementVector)
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, _distanceToWallCheck / 2, MovementVector, out hit, _distanceToWallCheck))
        {
            print(Vector3.Angle(hit.normal, MovementVector.normalized)/2);
            if (Vector3.Angle(hit.normal, MovementVector.normalized)/2 < _maxAngle)
            {
                // Adjust movement along the wall
                MovementVector = Vector3.ProjectOnPlane(MovementVector, hit.normal);
                Debug.DrawRay(hit.point, MovementVector.normalized, Color.red);
            }
            else
            {
                //stop if wall angle is too steep
                MovementVector = Vector3.zero;
            }
        }
        return MovementVector;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _moveDirection * Time.deltaTime, _distanceToWallCheck);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + _moveDirection.normalized * _distanceToWallCheck, _distanceToWallCheck / 2);
    }
}
