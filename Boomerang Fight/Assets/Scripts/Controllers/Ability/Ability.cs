using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected AbilityData abilityData;
    public abstract void UseAbility();
}
