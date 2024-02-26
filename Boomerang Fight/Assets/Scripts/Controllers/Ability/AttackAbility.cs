using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : Ability
{
    [Header("Boomerang Reference")]
    [SerializeField] private Boomerang _playerBoomerang;
    [Header("Attack Parameters")]
    [SerializeField] protected float _baseDamage;
    
    public Boomerang PlayerBoomerang { get { return _playerBoomerang; } }
}
