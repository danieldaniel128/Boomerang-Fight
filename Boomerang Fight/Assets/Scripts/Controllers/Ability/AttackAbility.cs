using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackAbility : Ability
{
    [Header("Boomerang Reference")]
    [SerializeField] protected Boomerang _playerBoomerang;
    
    public Boomerang PlayerBoomerang { get { return _playerBoomerang; } }
}
