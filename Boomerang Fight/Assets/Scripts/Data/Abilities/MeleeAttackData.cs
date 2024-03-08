using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAbilityData", menuName = "AbilityData/AttackAbility/MeleeAbilityData")]

public class MeleeAttackData : AbilityAttackData
{
    [Header("Melee Timing Parameters")]
    [SerializeField] protected float _timeToStartAttack;
    [SerializeField] protected float _attackDuration;

    #region Properties
    public float TimeToStartAttack { get { return _timeToStartAttack;} }
    public float AttackDuration { get { return _attackDuration;} }
    #endregion Properties
}
