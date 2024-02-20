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

    Vector3 _attackDirectionVector;

    Vector3 _releasedPosition;
    Vector3 _currentBoomerangFromReleasedPositionVector => _releasedPosition - PlayerBoomerang.transform.position;
    private float _currentDistanceFromReleasedPosition => _currentBoomerangFromReleasedPositionVector.magnitude; // need to be distance travelled

    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (!PlayerBoomerang.IsFlying && !PlayerBoomerang.IsSeperated)
            PlayerBoomerang.RB.velocity = Vector3.zero;
        if (PlayerBoomerang.Interrupted)
            return;
        Fly();
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
        PlayerBoomerang.SetBoomerangDamage(_baseDamage);
        timeFlying = 0;
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
        _baseDamage = rangeAbilityData.Damage;
    }
    #endregion Ability Overrides

    private void FlyBoomerangBack()
    {
        if (!PlayerBoomerang.IsFlying)
            return;

        //get distance and direction to player vector
        Vector3 distanceToPlayerVector = PlayerBoomerang.Parent.transform.position - PlayerBoomerang.transform.position;
        Vector3 boomerangVelocity = distanceToPlayerVector.normalized * _maxBoomerangSpeed;
        PlayerBoomerang.RB.velocity = boomerangVelocity; //attack time cant be 0
        
        if (distanceToPlayerVector.magnitude <= PlayerBoomerang.MinDistanceToPickUp)
        {
            print("time flying when returned: " + timeFlying);
            PlayerBoomerang.Stop();
        }
    }

    private void FlyBoomerangOut() // need to split into boomerang flying out and boomerang flying back... need to make boomerang flying back
    {
        if (!PlayerBoomerang.IsFlying)
            return;
        //set velocity of attacking. x is distance of player and released attack position, f(x) is velocity.
        Vector3 boomerangVelocity = _attackDirectionVector * _maxBoomerangSpeed * (1 - _attackSpeedCurve.Evaluate(_currentDistanceFromReleasedPosition / _maxAttackRange));//_maxAttackRange cant be 0
        PlayerBoomerang.RB.velocity = boomerangVelocity; //attack time cant be 0
        if (_currentDistanceFromReleasedPosition >= _maxAttackRange)
        {
            print("time flying when max range reached: " + timeFlying);
            PlayerBoomerang.ReachedMaxRange = true;
        }
    }

    float timeFlying = 0;
    private void Fly()
    {
        timeFlying += Time.deltaTime;
        if (PlayerBoomerang.ReachedMaxRange)
            FlyBoomerangBack();
        else
            FlyBoomerangOut();
    }

    public void CalculateAttackRange(Vector3 attackDirection)
    {
        //cant use attack when is seperated.
        if (PlayerBoomerang.IsSeperated)
            return;

        if (attackDirection != Vector3.zero)
        {
            _attackDirectionVector = attackDirection;
        }
        else
        {
            //brawlstars-like auto aim here
            //_attackDirectionVector = closest enemy direction
        }
    }

}
