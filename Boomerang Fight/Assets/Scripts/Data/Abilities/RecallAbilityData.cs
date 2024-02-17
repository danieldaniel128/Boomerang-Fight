using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecallAbilityData", menuName = "AbilityData/AttackAbility/RecallAbilityData")]
public class RecallAbilityData : AbilityAttackData
{
    [Header("Recall Limits")]
    [SerializeField] protected float _maxRecallBoomerangSpeed;
    [SerializeField] protected float _recallForce;
    [SerializeField] protected float _minDistanceToAttach;
    [SerializeField] protected float _autoBoomerangTeleportToPlayerTime;

    #region Properties

    public float MaxRecallBoomerangSpeed { get { return _maxRecallBoomerangSpeed; } }
    public float AutoBoomerangTeleportToPlayerTime { get { return _autoBoomerangTeleportToPlayerTime; } }
    public float RecallForce { get { return _recallForce; } }
    public float MinDistanceToRecallPosition { get { return _minDistanceToAttach; } }

    #endregion Properties
}
