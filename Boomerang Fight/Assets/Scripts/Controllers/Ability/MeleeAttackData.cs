using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeAttackData", menuName = "AttackData/MeleeAttackData")]
public class MeleeAttackData : CommonAttackData
{
    [Header("Melee Timing Parameters")]
    [SerializeField] private float _timeToStartAttack;
    [SerializeField] private float _attackDuration;
}
