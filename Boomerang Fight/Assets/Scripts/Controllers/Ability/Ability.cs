using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviourPun
{
    [SerializeField] protected AbilityData abilityData;
    [SerializeField] protected Animator animator;

    /// <summary>
    /// Gets the data from the ability scriptable object
    /// </summary>
    protected abstract void GetData();
    public abstract void UseAbility();

}
