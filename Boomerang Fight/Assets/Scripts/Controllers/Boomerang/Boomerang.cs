using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : MonoBehaviourPun
{
    [SerializeField] GameObject _parent;
    [Header("Components")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] Animator _animator;
    [Header("Boomerang Limits")]
    [SerializeField] float _slowDownForce;
    [SerializeField] float _minSpeedToDamage;
    [SerializeField] float _minDistanceToPickUp;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxRecallSpeed;
    [SerializeField] float _directionToPlayerAngleThreshHold;
    [SerializeField] float _directionToPlayerAngleThreshHoldWhileRecalling;
    [SerializeField] float _returnToParentForce;
    [SerializeField] AnimationCurve _speedCurve;
    [SerializeField] LayerMask _canAttackLayerMask;
    [Header("Ability Parameters")]
    float _range;
    Vector3 _launchDirection;
    float _damage;
    [Header("Boomerang Information")]
    float _distanceTravelled;
    float _currentSpeed;
    float _currentRecallForce;
    bool _interrupted;
    bool _damaging;
    bool _reachedMaxDistance;
    bool _attachable;
    bool _recalling;
    bool _grounded;
    [Header("Events")]
    public Action OnAttach;
    public Action OnRelease;


    Vector3 DirectionToParent => _parent.transform.position - transform.position;
    public bool ReachedMaxDistance { get => _reachedMaxDistance; }
    public Rigidbody RB { get => _rb; }
    public float MaxSpeed { get => _recalling ? _maxRecallSpeed : _maxSpeed; }

    /// <summary>
    /// while recalling, 
    /// the angle considered moving towards is narrower meaning the boomerang needs to move more in the direction of the player to be considererd moving towards player
    /// </summary>
    float DirectionToParentAngleThreshHold => _recalling ? _directionToPlayerAngleThreshHoldWhileRecalling : _directionToPlayerAngleThreshHold;

    bool Grounded
    {
        get => _grounded;
        set
        {
            _animator.SetBool("grounded", value);
            _grounded = value;
        }
    }

    private void FixedUpdate()
    {
        TryAttach();
        CheckDamaging();
        Fly();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (!_damaging)
            return;
        if (other.attachedRigidbody != null)
            if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.attachedRigidbody.gameObject.layer)))
            {
                other.attachedRigidbody.gameObject.GetComponent<Health>().TakeDamage(_damage);
                other.attachedRigidbody.gameObject.GetComponent<PlayerController>().VFXTransitioner.ActivateVFX(VFXTypeEnum.HittingEnemy);
            }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_reachedMaxDistance)
            _interrupted = true;
    }
    private void ResetBoomerangInformation()
    {
        _interrupted = false;
        _reachedMaxDistance = false;
        _attachable = false;
        Grounded = false;
        _distanceTravelled = 0;
    }

    public void Release(Vector3 directionVector, float damage)
    {
        //remove game object from parent
        photonView.RPC(nameof(ReleaseRPC), RpcTarget.All, directionVector, damage);
    }


    public void Attach()
    {

        photonView.RPC(nameof(AttachRPC), RpcTarget.All);
        //gameObject.SetActive(false);
    }
    [PunRPC]
    private void AttachRPC()
    {
        ResetBoomerangInformation();
        //set boomerang body parent to its parent holder.
        transform.SetParent(_parent.transform);
        //set position
        transform.localPosition = Vector3.zero;
        //stop movement
        Stop();
        //deactivate logic boomerang
        OnAttach?.Invoke();
        gameObject.SetActive(false);
    }
    [PunRPC]
    private void ReleaseRPC(Vector3 directionVector, float damage)
    {
        gameObject.SetActive(true);
        //set range, direction and damage
        _range = directionVector.magnitude;
        _launchDirection = directionVector.normalized;
        _damage = damage;
        //reset parameters
        ResetBoomerangInformation();
        //activate logic boomerang
        _rb.velocity = directionVector;
        transform.SetParent(null);
        OnRelease?.Invoke();
    }
    private void TryAttach()
    {
        if (!_attachable) return;
        if (Vector3.Distance(transform.position, _parent.transform.position) < _minDistanceToPickUp)
            Attach();
    }

    private void Fly()
    {
        CalculateDistanceTravelled();
        if (!_interrupted)
            FlyUninterrupted();
        else
        {
            if (_recalling)
                AddRecallForce();
            else
                SlowDown();
        }
    }

    private void FlyUninterrupted()
    {
        CalculateUninterruptedSpeed();
        Vector3 newVelocity = Vector3.zero;
        if (_reachedMaxDistance)
        {

            Vector3 directionToPlayer = (_parent.transform.position - transform.position).normalized;

            Vector3 forceToApply = directionToPlayer * _returnToParentForce;

            Vector3 forcedVelocity = _rb.velocity + forceToApply * Time.fixedDeltaTime;

            newVelocity = Vector3.ClampMagnitude(forcedVelocity, MaxSpeed);

        }
        else
        {
            newVelocity = _rb.velocity.normalized * _currentSpeed;

        }

        _rb.velocity = newVelocity;

    }

    private void CalculateUninterruptedSpeed()
    {
        float distanceRatio = _distanceTravelled / _range;
        if (_reachedMaxDistance)
            distanceRatio = 1 - distanceRatio;

        float speedCurveModifier = Mathf.Clamp(_speedCurve.Evaluate(distanceRatio), 0.05f, 0.95f);
        float newSpeed = speedCurveModifier * MaxSpeed;

        if (MovingTowardsPlayer())
        {
            //if moving towards player then dont slow down.
            if (newSpeed > _currentSpeed)
            {
                _currentSpeed = newSpeed;
            }
        }
        else
        {
            _currentSpeed = newSpeed;
        }
    }
    bool MovingTowardsPlayer()
    {
        float dotProduct = Vector3.Dot(_rb.velocity.normalized, DirectionToParent.normalized);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle < DirectionToParentAngleThreshHold / 2;
    }

    public void Recall(float recallForce)
    {

        if (!photonView.IsMine)
            return;
        Grounded = false;
        _recalling = true;
        _currentRecallForce = recallForce;
        //checks if close enough to end Recall.
        TryAttach();
    }

    private void AddRecallForce()
    {
        Vector3 recallForceToApply = DirectionToParent.normalized * _currentRecallForce;
        recallForceToApply = MovingTowardsPlayer() ? recallForceToApply : recallForceToApply * 2;

        Vector3 recallVelocity = _rb.velocity + recallForceToApply * Time.fixedDeltaTime;

        recallVelocity = Vector3.ClampMagnitude(recallVelocity, MaxSpeed);

        _rb.velocity = recallVelocity;
    }

    public void StopRecall()
    {
        _recalling = false;
    }

    public bool CanRecall()
    {
        return _reachedMaxDistance && _interrupted;
    }

    private void CalculateDistanceTravelled()
    {
        _distanceTravelled += _rb.velocity.magnitude * Time.fixedDeltaTime;
        if (_reachedMaxDistance) return;


        _attachable = _distanceTravelled > _minDistanceToPickUp + 0.5f;
        _reachedMaxDistance = _distanceTravelled >= _range ? true : false;

        if (_reachedMaxDistance)
        {
            _distanceTravelled = 0;
        }
    }

    private void CheckDamaging()
    {
        _damaging = _rb.velocity.magnitude > _minSpeedToDamage;
    }

    private void SlowDown()
    {
        if (Grounded)
            return;

        if (_rb.velocity.magnitude <= 0.5f)
            Stop();
        else
            _rb.AddForce(-_rb.velocity.normalized * _slowDownForce);
    }

    private void Stop()
    {
        _rb.velocity = Vector3.zero;
        Grounded = true;
    }

}
