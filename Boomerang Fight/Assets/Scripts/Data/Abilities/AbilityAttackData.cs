using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityAttackData : AbilityData
{
    [Header("Attack Parameters")]
    [SerializeField] private LayerMask _canAttackLayerMask;
    [SerializeField] private float _damage;

    #region Properties
    public LayerMask CanAttackLayerMask { get { return _canAttackLayerMask; } }
    public float Damage { get { return _damage; } }
    #endregion Properties
}
