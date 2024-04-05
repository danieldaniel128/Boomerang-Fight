using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using static UnityEngine.GraphicsBuffer;

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
    bool _onCooldown = false;

    CountdownTimer _cooldownTimer;
    CountdownTimer _delayAttackTimer;
    CountdownTimer _attackDurationTimer;

    public Action OnAttack;

    //Gizmo Parameters
    [Header("Gizmos")]
    [SerializeField] bool _showGizmos;
    bool _inAttack = false;

    public bool InAttack => _inAttack;

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
        if (other.gameObject.layer == 6)
        {
            print("Hit Boomerang");
            return;
        }

        if (other.attachedRigidbody != null)
            if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.attachedRigidbody.gameObject.layer)))
            {
                other.attachedRigidbody.gameObject.GetComponent<Health>().TakeDamage(Damage);
            }
        //HitTarget(other);
    }

    #region Ability Overrides

    [ContextMenu("Press Attack")]
    public override void UseAbility()
    {
        if (photonView.IsMine) //maybe not necessary?
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
        _cooldownTimer.OnTimerStop += () => _onCooldown = false;
        _cooldownTimer.OnTimerStart += () => _onCooldown = true;
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
        if (!_canAttack)
            return;

        if (_onCooldown)
            return;

        if (PlayerBoomerang.gameObject.activeInHierarchy)
            return;

        OnAttack?.Invoke();
        _delayAttackTimer.Start();
        _cooldownTimer.Start();
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

    public void EnableAttack()
    {
        _canAttack = true;
    }

    public void DisableAttack()
    {
        _canAttack = false;
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
