using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IPunInstantiateMagicCallback
{

    //[SerializeField] UserInputPhone _input;
    [SerializeField] GameObject _playerBody;
    [Header("Components")]
    [SerializeField] CameraFollow _cameraFollow;
    [SerializeField] RangeAbility _rangeAbility;
    [SerializeField] RecallAbility _recallAbility;

    [Header("JoySticks Set-UP")]
    [SerializeField] GameObject _joystickCanvas;
    [SerializeField] Joystick _moveJoystick;
    [SerializeField] Joystick _AttackJoystick;
    [Header("Player Stats")]
    [SerializeField] float _moveSpeed;

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
        OnLocalPlayerControllerUpdate += HandleMovement;
        //when press attack button, enable attack.
        _AttackJoystick.OnJoystickDown += EnableRangeAbility;
        _AttackJoystick.OnJoystickDown += EnableRecallAbility;

    }
    private void OnDisable()
    {
        //In order to prevent resource leaks, unsubscribe events
        OnLocalPlayerControllerUpdate -= HandleMovement;
        _AttackJoystick.OnJoystickDown -= EnableRangeAbility;
        _AttackJoystick.OnJoystickDown -= EnableRecallAbility;
        DisableRangeAbility();
        DisableRecallAbility();
    }

    private void Update()
    {
        LocalPlayerControlUpdate();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveJoystick.Horizontal, 0, _moveJoystick.Vertical).normalized;
        transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        //do logic
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
        if (_recallAbility.CanRecall())
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
    }
    private void DisableRecallAbility()
    {
        _AttackJoystick.OnJoystickPressed -= HandleRecall;
    }
    #endregion Recall Ability

    #region Range Ability
    private void UseRangeAbility()
    {
        if (!photonView.IsMine)
            return;

        _rangeAbility.UseAbility();
    }
    private void HandleRangeAbility()
    {
        if (!photonView.IsMine)
            return;

        Vector3 attackDirection = new Vector3(_AttackJoystick.Horizontal, 0, _AttackJoystick.Vertical).normalized;
        _rangeAbility.CalculateAttackRange(attackDirection);
    }
    private void EnableRangeAbility()
    {
        _AttackJoystick.OnJoystickUp += UseRangeAbility;
        _AttackJoystick.OnJoystickDrag += HandleRangeAbility;
    }
    private void DisableRangeAbility()
    {
        _AttackJoystick.OnJoystickUp -= UseRangeAbility;
        _AttackJoystick.OnJoystickDrag -= HandleRangeAbility;
    }
    #endregion Range Ability

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        MultiplayerPlayerSpawner.Instance.SetMyPlayerController(this);
        MultiplayerPlayerSpawner.Instance.RegisterPlayerController(this);
    }
}
