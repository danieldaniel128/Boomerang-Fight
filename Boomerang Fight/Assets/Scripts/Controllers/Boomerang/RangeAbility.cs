using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAbility : AttackAbility//interface of attacks
{
    [Header("Range Ability Limits")]
    float _maxAttackRange;
    float _minAttackRange;
    float _maxChargeTime;
    [Header("Actions")]
    public Action OnBoomerangReleased;

    Vector3 _attackDirectionVector = Vector3.forward;

    public bool Aimed {  get; set; }

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        GetData();
    }

    #region Ability Overrides
    public override void UseAbility() //on joystick up
    {
        Aimed = false;
        if(!PlayerBoomerang.gameObject.activeInHierarchy)
        {
            PlayerBoomerang.Release(_attackDirectionVector * _maxAttackRange, _baseDamage);
        }
    }
    protected override void GetData()
    {
        RangeAttackData rangeAbilityData = abilityData as RangeAttackData;

        _maxAttackRange = rangeAbilityData.MaxAttackRange;
        _minAttackRange = rangeAbilityData.MinAttackRange;
        _maxChargeTime = rangeAbilityData.MaxChargeTime;
        _baseDamage = rangeAbilityData.Damage;


        //in a comment so ill remember to move these to boomerang:

        //_maxBoomerangSpeed = rangeAbilityData.MaxBoomerangSpeed;
        //_canAttackLayerMask = rangeAbilityData.CanAttackLayerMask;
        //_attackSpeedCurve = rangeAbilityData.AttackSpeedCurve;
    }
    #endregion Ability Overrides

    public void CalculateAttackDirection(Vector3 attackDirection)
    {
        //cant use attack when boomerang is disabled.
        if (PlayerBoomerang.isActiveAndEnabled)
            return;

        if (attackDirection != Vector3.zero)
        {
            _attackDirectionVector = attackDirection;
        }
        else
        {
            //brawlstars-like auto aim here
            //_attackDirectionVector = closest enemy direction
            print("attack nearest enemy or facing");
        }
    }

}
