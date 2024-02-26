using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    [Header("Ability Parameters")]
    [SerializeField] protected float _cooldown;

    #region Properties
    public float Cooldown { get { return _cooldown; } }
    #endregion Properties
}
