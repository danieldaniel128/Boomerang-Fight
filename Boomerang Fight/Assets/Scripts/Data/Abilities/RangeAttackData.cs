using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAbilityData", menuName = "AbilityData/AttackAbility/RangeAbilityData")]
public class RangeAttackData : AbilityAttackData
{
    [Header("Boomerang Limits")]
    [SerializeField] protected float _maxAttackRange;
    [SerializeField] protected float _minAttackRange;
    [SerializeField] protected float _timeTillMaxCharge;

    [Header("Throw Behaviors")]
    [SerializeField] protected AnimationCurve _attackSpeedCurve;

    #region Properties
    public float MaxAttackRange { get { return _maxAttackRange; } }
    public float MinAttackRange { get { return _minAttackRange; } }
    public float TimeTillMaxCharge { get { return _timeTillMaxCharge; } }
    public AnimationCurve AttackSpeedCurve { get { return _attackSpeedCurve; } }
    #endregion Properties
}
