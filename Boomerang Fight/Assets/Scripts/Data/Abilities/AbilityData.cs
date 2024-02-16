using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    //[Header("Ability Components")]
    //[SerializeField] protected Animator _animator;

    [Header("Ability Parameters")]
    [SerializeField] protected float _cooldown;

    #region Properties
    //public Animator Animator { get { return _animator; } }
    public float Cooldown { get { return _cooldown; } }
    #endregion Properties
}
