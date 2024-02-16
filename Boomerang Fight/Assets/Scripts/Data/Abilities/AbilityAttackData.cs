using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityAttackData : AbilityData
{
    [Header("Attack Parameters")]
    [SerializeField] protected LayerMask _canAttackLayerMask;
    [SerializeField] protected float _damage;

    #region Properties
    public LayerMask CanAttackLayerMask { get { return _canAttackLayerMask; } }
    public float Damage { get { return _damage; } }
    #endregion Properties
}
