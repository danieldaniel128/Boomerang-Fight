using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class BoomerangMeleeAttack : MonoBehaviourPun
{
    private const string MELEE_ATTACK_STRING_CONST = "MeleeAttack";

    public Action OnAttackPressed { get; set; }

    [Header("Collider Parameters")]
    [SerializeField] BoxCollider _attackCollider;
    [SerializeField] LayerMask _canAttackLayerMask;

    [Header("Timing Parameters")]
    [SerializeField] float _timeToStartAttack;
    [SerializeField] float _attackDuration;
    [SerializeField] float _cooldownDuration;

    [Header("Components")]
    [SerializeField] Animator _characterAnimator;

    bool _canAttack = true;

    CountdownTimer _cooldownTimer;
    CountdownTimer _delayAttackTimer;
    CountdownTimer _attackDurationTimer;

    //Gizmo Parameters
    [SerializeField] bool _showGizmos;
    bool _inAttack = false;


    private void Start()
    {
        //set up cooldown timer
        _cooldownTimer = new(_cooldownDuration);
        _cooldownTimer.OnTimerStop += () => _canAttack = true;

        //set up delay to attack timer
        _delayAttackTimer = new(_timeToStartAttack);
        _delayAttackTimer.OnTimerStop += Attack;

        //set up duration of attack timer
        _attackDurationTimer = new(_attackDuration);
        _attackDurationTimer.OnTimerStop += StopAttack;

    }
    private void OnEnable()
    {
        OnAttackPressed += TryInitiateAttack;
    }
    private void OnDisable()
    {
        OnAttackPressed -= TryInitiateAttack;
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

    void Tick()
    {
        _delayAttackTimer.Tick(Time.deltaTime);
        _cooldownTimer.Tick(Time.deltaTime);
        _attackDurationTimer.Tick(Time.deltaTime);
    }

    [ContextMenu("Press Attack")]
    public void ActivateAttackEvent()
    {
        //print("Attack invoked");
        OnAttackPressed?.Invoke();
    }

    public void TryInitiateAttack()
    {
        if (_canAttack)
        {
            _canAttack = false;
            //_characterAnimator.Play(MELEE_ATTACK_STRING_CONST);
            _delayAttackTimer.Start();
            _cooldownTimer.Start();
        }

    }

    public void Attack()
    {
        //enable collider
        _attackDurationTimer.Start();
        _attackCollider.enabled = true;
        _inAttack = true;

    }

    public void StopAttack()
    {
        _attackCollider.enabled = false;
        _inAttack = false;
    }

    public void HitTarget(Collider target)
    {
        target.GetComponentInParent<Health>().TakeDamage(2);
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
