using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallAbility : AttackAbility
{
    public Action OnFinishRecalling { get; set; }
    [SerializeField] Transform _recallObject;

    [Header("RecallData")]
    float _autoBoomerangTeleportToPlayerTime;
    float _recallForce;


    private void Start()
    {
        if (!photonView.IsMine)
            return;
        GetData();
    }

    #region Ability Overrides
    protected override void GetData()
    {
        // cast to recall ability type
        RecallAbilityData recallAbilityData = abilityData as RecallAbilityData;

        _autoBoomerangTeleportToPlayerTime = recallAbilityData.AutoBoomerangTeleportToPlayerTime;
        _recallForce = recallAbilityData.RecallForce;

        //comment so ill remember to delete later
        //_maxRecallBoomerangSpeed = recallAbilityData.MaxBoomerangRecallSpeed;
        //_minDistanceToRecallPosition = recallAbilityData.MinDistanceToRecallPosition;
    }
    public override void UseAbility()
    {
        PlayerBoomerang.Recall(_recallForce);
    }
    #endregion #region Ability Overrides

}
