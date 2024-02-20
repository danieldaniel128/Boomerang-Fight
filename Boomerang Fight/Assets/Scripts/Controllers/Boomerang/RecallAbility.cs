using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallAbility : AttackAbility
{
    public Action OnFinishRecalling { get; set; }
    [SerializeField] Transform _recallObject;

    [Header("RecallData")]
    float _maxRecallBoomerangSpeed;
    float _autoBoomerangTeleportToPlayerTime;
    float _recallForce;
    float _minDistanceToRecallPosition;

    Vector3 _currentBoomerangVectorToRecallPosition => _recallObject.position - PlayerBoomerang.transform.position;
    Vector3 _currentDirectionToRecallPosition => _currentBoomerangVectorToRecallPosition.normalized;
    float _currentBoomerangDistanceFromRecallPos => _currentBoomerangVectorToRecallPosition.magnitude;

    private void Start()
    {
        GetData();
    }

    #region Ability Overrides
    protected override void GetData()
    {
        // Cast abilityData to the appropriate scriptable object type
        RecallAbilityData recallAbilityData = abilityData as RecallAbilityData;

        // Assigning variables from the scriptable object
        _maxRecallBoomerangSpeed = recallAbilityData.MaxBoomerangRecallSpeed;
        _autoBoomerangTeleportToPlayerTime = recallAbilityData.AutoBoomerangTeleportToPlayerTime;
        _recallForce = recallAbilityData.RecallForce;
        _minDistanceToRecallPosition = recallAbilityData.MinDistanceToRecallPosition;
    }
    public override void UseAbility()
    {
        Recall();
    }
    #endregion #region Ability Overrides

    public bool CanRecall()
    {
        //is not in hand and not flying?
        if (!PlayerBoomerang.IsFlying && PlayerBoomerang.IsSeperated)
            return true;
        return false;
    }

    private void Recall()
    {
        //TODO check if isMine should be on the boomerang
        if (!photonView.IsMine)
            return;
        AddRecallForce();
        //checks if close enough to end Recall.
        if (_currentBoomerangDistanceFromRecallPos <= _minDistanceToRecallPosition)
            AttachBoomerang();
    }

    private void AddRecallForce()
    {
        if (!PlayerBoomerang.IsSeperated)
            return;
        //calculate how much force to add so it doesnt go over max speed
        float forceToAdd = _recallForce;
        if (PlayerBoomerang.RB.velocity.magnitude >= _maxRecallBoomerangSpeed)
            forceToAdd = 0;

        //add force in the direction of recall position
        PlayerBoomerang.RB.AddForce(_currentDirectionToRecallPosition * forceToAdd);
    }
    private void AttachBoomerang()
    {
        //finished recalling.
        OnFinishRecalling?.Invoke();
        //stop timer.
        PlayerBoomerang.Attach();
    }

}
