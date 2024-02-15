using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommonAttackData : ScriptableObject
{
    [Header("Basic Attack Componentes")]
    [SerializeField] protected Animator _animator;

    [Header("Basic Attack Parameters")]
    [SerializeField] protected LayerMask _canAttackLayerMask;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _cooldown;
}
