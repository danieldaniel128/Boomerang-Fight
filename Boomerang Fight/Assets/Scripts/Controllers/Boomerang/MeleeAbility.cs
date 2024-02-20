using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class MeleeAbility : AttackAbility
{
    private const string MELEE_ATTACK_STRING_CONST = "MeleeAttack";

    public float Damage { get => _baseDamage; }

    [Header("Collider Parameters")]
    [SerializeField] BoxCollider _attackCollider;
    LayerMask _canAttackLayerMask;

    [Header("Timing Parameters")]
    float _timeToStartAttack;
    float _attackDuration;
    float _cooldownDuration;

    bool _canAttack = true;

    CountdownTimer _cooldownTimer;
    CountdownTimer _delayAttackTimer;
    CountdownTimer _attackDurationTimer;


    //Gizmo Parameters
    [SerializeField] bool _showGizmos;
    bool _inAttack = false;


    private void Start()
    {
        GetData();
        InitializeTimers();
    }
    private void Update()
    {
        Tick();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.gameObject.layer)))
        {
            HitTarget(other);
        }
    }

    #region Ability Overrides

    [ContextMenu("Press Attack")]
    public override void UseAbility()
    {
        TryInitiateAttack();
    }
    protected override void GetData()
    {
        MeleeAttackData meleeAttackData = abilityData as MeleeAttackData;

        _canAttackLayerMask = meleeAttackData.CanAttackLayerMask;
        _timeToStartAttack = meleeAttackData.TimeToStartAttack;
        _attackDuration = meleeAttackData.AttackDuration;
        _cooldownDuration = meleeAttackData.Cooldown;
        _baseDamage = meleeAttackData.Damage;
    }

    #endregion Ability Overrides

    #region Timers
    private void InitializeTimers()
    {
        //set up cooldown timer
        InitializeCooldownTimer();

        //set up delay to attack timer
        InitializeDelayTimer();

        //set up duration of attack timer
        InitializeDurationTimer();
    }

    private void InitializeDurationTimer()
    {
        _attackDurationTimer = new(_attackDuration);
        _attackDurationTimer.OnTimerStop += StopAttack;
    }

    private void InitializeDelayTimer()
    {
        _delayAttackTimer = new(_timeToStartAttack);
        _delayAttackTimer.OnTimerStop += Attack;
    }

    private void InitializeCooldownTimer()
    {
        _cooldownTimer = new(_cooldownDuration);
        _cooldownTimer.OnTimerStop += () => _canAttack = true;
    }

    void Tick()
    {
        _delayAttackTimer.Tick(Time.deltaTime);
        _cooldownTimer.Tick(Time.deltaTime);
        _attackDurationTimer.Tick(Time.deltaTime);
    }
    #endregion Timers

    private void TryInitiateAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            _delayAttackTimer.Start();
            _cooldownTimer.Start();
        }
    }

    private void Attack()
    {
        //enable collider
        _attackDurationTimer.Start();
        _attackCollider.enabled = true;
        _inAttack = true;

    }

    private void StopAttack()
    {
        _attackCollider.enabled = false;
        _inAttack = false;
    }

    public void HitTarget(Collider target)
    {
        target.GetComponentInParent<Health>().TakeDamage(Damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos)
            return;

        if (_canAttack)
            Gizmos.color = Color.green;
        else
        {
            if (_inAttack)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;
        }

        //convert from local to world space for draw wire cube
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(_attackCollider.center, _attackCollider.size);
    }
}
