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
    [Header("Boomerang Limits")]
    [SerializeField] float _slowDownForce;
    [SerializeField] float _minSpeedToDamage;
    [SerializeField] float _minDistanceToPickUp;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _directionToPlayerAngleThreshHold;
    [SerializeField] float _maxRecallSpeed;
    [SerializeField] AnimationCurve _speedCurve;
    [SerializeField] LayerMask _canAttackLayerMask;
    [Header("Ability Parameters")]
    float _range;
    Vector3 _launchDirection;
    float _damage;
    Vector3 _directionToParent => _parent.transform.position - transform.position;
    [Header("Boomerang Information")]
    float _distanceTravelled;
    float _currentSpeed;
    bool _interrupted;
    bool _damaging;
    bool _reachedMaxDistance;
    bool _attachable;
    bool _recalling;
    [Header("Events")]
    public Action OnAttach;
    public Action OnRelease;


    public bool ReachedMaxDistance { get => _reachedMaxDistance; }
    public Rigidbody RB { get => _rb; }
    public float MaxSpeed { get => _recalling ? _maxRecallSpeed : _maxSpeed; }

    private void FixedUpdate()
    {
        TryAttach();
        CheckDamaging();
        Fly();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine) return;

        if (!_damaging)
            return;
        if(other.attachedRigidbody!=null)
            if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.attachedRigidbody.gameObject.layer)))
            {
                other.attachedRigidbody.gameObject.GetComponent<Health>().TakeDamage(_damage);
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
            if (!_recalling)
                SlowDown();
        }
    }

    private void FlyUninterrupted()
    {
        CalculateUninterruptedSpeed();
        Vector3 newVelocity = Vector3.zero;
        if (_reachedMaxDistance)
            newVelocity = Vector3.Normalize(_parent.transform.position - transform.position);
        else
            newVelocity = _rb.velocity.normalized;

        newVelocity *= _currentSpeed;

        _rb.velocity = newVelocity;
    }

    private void CalculateUninterruptedSpeed()
    {
        float distanceRatio = _distanceTravelled / _range;
        if (_reachedMaxDistance)
            distanceRatio = 1 - distanceRatio;
        //if moving towards player then dont slow down.
        //if next speed is faster or slower, 
        //if moving towards player
        float speedCurveModifier = Mathf.Clamp(_speedCurve.Evaluate(distanceRatio), 0.05f, 0.95f);
        float newSpeed = speedCurveModifier * MaxSpeed;

        if (MovingTowardsPlayer())
        {
            if(newSpeed > _currentSpeed)
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
        float dotProduct = Vector3.Dot(_rb.velocity.normalized, _directionToParent.normalized);
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        return angle < _directionToPlayerAngleThreshHold / 2;
    }

    public void Recall(float recallForce)
    {
        //TODO check if isMine should be on the boomerang
        if (!photonView.IsMine)
            return;

        _recalling = true;
        AddRecallForce(recallForce);
        //checks if close enough to end Recall.
        TryAttach();
    }

    private void AddRecallForce(float recallForce)
    {
        //calculate how much force to add so it doesnt go over max speed
        float forceToAdd = recallForce * Time.deltaTime;
        if (_rb.velocity.magnitude >= MaxSpeed)
            forceToAdd = 0;
        //add force in the direction of recall position
        _rb.AddForce(_directionToParent * forceToAdd);
    }

    public void StopRecall()
    {
        _recalling = false;
    }

    public bool CanRecall()
    {
        if (_reachedMaxDistance)
            return true;
        return false;
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
        if (_rb.velocity.magnitude <= 0.5f)
            Stop();
        else
            _rb.AddForce(-_rb.velocity.normalized * _slowDownForce);
    }

    private void Stop()
    {
        _rb.velocity = Vector3.zero;
    }

}
