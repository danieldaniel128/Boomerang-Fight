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
    float _timeTillMaxCharge;
    float _currentRange;
    float _chargeTimer = 0;
    [Header("Actions")]
    public Action OnBoomerangReleased;

    Vector3 _attackDirectionVector = Vector3.forward;
    StopwatchTimer _chargeStopwatch;
    public bool Aimed {  get; set; }

    private void Start()
    {
        if (!photonView.IsMine)
            return;
        GetData();
        _chargeStopwatch = new StopwatchTimer();
    }

    private void Update()
    {
        _chargeStopwatch.Tick(Time.deltaTime);
    }

    #region Ability Overrides
    public override void UseAbility() //on joystick up
    {
        Aimed = false;
        if(!PlayerBoomerang.gameObject.activeInHierarchy)
        {
            CalculateCharge();
            PlayerBoomerang.Release(_attackDirectionVector * _currentRange, _baseDamage);
            _chargeStopwatch.Stop();
        }
    }
    protected override void GetData()
    {
        RangeAttackData rangeAbilityData = abilityData as RangeAttackData;

        _maxAttackRange = rangeAbilityData.MaxAttackRange;
        _minAttackRange = rangeAbilityData.MinAttackRange;
        _timeTillMaxCharge = rangeAbilityData.TimeTillMaxCharge;
        _baseDamage = rangeAbilityData.Damage;
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

    }

    public void StartCharge()
    {
        _chargeStopwatch.Reset();
        _chargeStopwatch.Start();
        _currentRange = _minAttackRange;
    }

    public void AddCharge()
    {
        if(_chargeTimer < _timeTillMaxCharge)
            _chargeTimer += Time.deltaTime;

        _currentRange = Mathf.Lerp(_minAttackRange, _maxAttackRange, Mathf.Clamp01(_chargeTimer/_timeTillMaxCharge));
    }

    private void CalculateCharge()
    {
        _currentRange = Mathf.Lerp(_minAttackRange, _maxAttackRange, Mathf.Clamp01(_chargeStopwatch.Time / _timeTillMaxCharge));
    }

}
