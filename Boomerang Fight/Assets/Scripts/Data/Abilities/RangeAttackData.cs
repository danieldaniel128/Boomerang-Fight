using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAbilityData", menuName = "AbilityData/AttackAbility/RangeAbilityData")]
public class RangeAttackData : AbilityAttackData
{
    [Header("Boomerang Limits")]
    [SerializeField] protected float _maxBoomerangSpeed;
    [SerializeField] protected float _maxAttackRange;
    [SerializeField] protected float _minAttackRange;
    [SerializeField] protected float _maxChargeTime;

    [Header("Throw Behaviors")]
    [SerializeField] protected AnimationCurve _attackSpeedCurve;

    #region Properties
    public float MaxBoomerangSpeed { get { return _maxBoomerangSpeed; } }
    public float MaxAttackRange { get { return _maxAttackRange; } }
    public float MinAttackRange { get { return _minAttackRange; } }
    public float MaxChargeTime { get { return _maxChargeTime; } }
    public AnimationCurve AttackSpeedCurve { get { return _attackSpeedCurve; } }
    #endregion Properties
}
