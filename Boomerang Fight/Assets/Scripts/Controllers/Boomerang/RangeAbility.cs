using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAbility : AttackAbility//interface of attacks
{
    public Action OnFinishRecalling { get; set; }
    LayerMask _canAttackLayerMask;
    [Header("Boomerang Behaviors")]
    AnimationCurve _attackSpeedCurve;
    [Header("Boomerang Limits")]
    float _maxBoomerangSpeed;
    float _maxAttackRange;
    float _minAttackRange;
    float _maxChargeTime;
    float _damage;

    Vector3 attackDirectionVector;
    Vector3 _releasedPosition;
    Vector3 _currentBoomerangFromReleasedPositionVector => _releasedPosition - PlayerBoomerang.transform.position;
    private float _currentDistanceFromReleasedPosition => _currentBoomerangFromReleasedPositionVector.magnitude; // need to change to from player starting point

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (!PlayerBoomerang.IsFlying && !PlayerBoomerang.IsSeperated)
            PlayerBoomerang.RB.velocity = Vector3.zero;
        FlyBoomerang();
    }
    private void Start()
    {
        if (!photonView.IsMine)
            return;
        PlayerBoomerang.RB.velocity = Vector3.zero;
        GetData();
        PlayerBoomerang.SetAttackLayerMask(_canAttackLayerMask);
    }

    #region Ability Overrides
    public override void UseAbility() //on joystick up
    {
        _releasedPosition = transform.position;
        PlayerBoomerang.IsFlying = true;
        PlayerBoomerang.SetBoomerangDamage(_damage);
        PlayerBoomerang.Release();
    }
    protected override void GetData()
    {
        RangeAttackData rangeAbilityData = abilityData as RangeAttackData;

        _canAttackLayerMask = rangeAbilityData.CanAttackLayerMask;
        _maxBoomerangSpeed = rangeAbilityData.MaxBoomerangSpeed;
        _maxAttackRange = rangeAbilityData.MaxAttackRange;
        _minAttackRange = rangeAbilityData.MinAttackRange;
        _maxChargeTime = rangeAbilityData.MaxChargeTime;
        _attackSpeedCurve = rangeAbilityData.AttackSpeedCurve;
        _damage = rangeAbilityData.Damage;
    }
    #endregion Ability Overrides

    private void FlyBoomerang() // need to split into boomerang flying out and boomerang flying back... need to make boomerang flying back
    {
        if (!PlayerBoomerang.IsFlying)
            return;
        //set velocity of attacking. x is distance of player and released attack position, f(x) is velocity.
        Vector3 boomerangVelocity = attackDirectionVector * _maxBoomerangSpeed * (1 - _attackSpeedCurve.Evaluate(_currentDistanceFromReleasedPosition / _maxAttackRange));//_maxAttackRange cant be 0
        PlayerBoomerang.RB.velocity = boomerangVelocity; //attack time cant be 0
        if (_currentDistanceFromReleasedPosition >= _maxAttackRange)
            PlayerBoomerang.Stop();
    }
    public void CalculateAttackRange(Vector3 attackDirection)
    {
        //cant use attack when is seperated.
        if (PlayerBoomerang.IsSeperated)
            return;

        if (attackDirection != Vector3.zero)
        {
            attackDirectionVector = attackDirection;
        }
        else
        {
            //brawl stars auto aim here
            //transform forward = closest enemy direction
        }
    }

}
