using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttackData", menuName = "AbilityData/AttackData/MeleeAttackData")]
public class MeleeAttackData : AbilityAttackData
{
    [Header("Melee Timing Parameters")]
    [SerializeField] protected float _timeToStartAttack;
    [SerializeField] protected float _attackDuration;

    public float TimeToStartAttack { get { return _timeToStartAttack;} }
    public float AttackDuration { get { return _attackDuration;} }
}
