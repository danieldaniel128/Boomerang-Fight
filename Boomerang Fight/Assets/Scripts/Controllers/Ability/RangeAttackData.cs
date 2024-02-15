using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangeAttackData", menuName = "AttackData/RangeAttackData")]
public class RangeAttackData : CommonAttackData
{
    [Header("Boomerang Limits")]
    [SerializeField] float _maxBoomerangSpeed;
    [SerializeField] float _maxAttackRange;
    [SerializeField] float _maxChargeTime;
    [Header("Boomerang Behaviors")]
    [SerializeField] AnimationCurve _attackSpeedCurve;
}
