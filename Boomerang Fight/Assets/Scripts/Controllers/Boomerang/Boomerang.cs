using Photon.Pun;
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
    [SerializeField] AnimationCurve _speedCurve;
    [SerializeField] LayerMask _canAttackLayerMask;
    [Header("Ability Parameters")]
    float _range;
    Vector3 _launchDirection;
    float _damage;
    [Header("Boomerang Information")]
    float _distanceTravelled;
    float _currentSpeed;
    bool _interrupted;
    bool _damaging;
    bool _reachedMaxDistance;
    bool _attachable;

    private void FixedUpdate()
    {
        if (!gameObject.activeInHierarchy)
            return;
        TryAttach();
        CheckDamaging();
        Fly();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_damaging)
            return;

        if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(_damage);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_reachedMaxDistance)
            _interrupted = true;
    }

    public void Release(Vector3 directionVector, float damage)
    {
        //set range, direction and damage
        _range = directionVector.magnitude;
        _launchDirection = directionVector.normalized;
        _damage = damage;
        //reset parameters
        _interrupted = false;
        _reachedMaxDistance = false;
        _attachable = false;
        _distanceTravelled = 0;
        //activate logic boomerang
        gameObject.SetActive(true);
        _rb.velocity = directionVector;
        print("Boomerang Released");
    }

    public void Attach()
    {
        print("Boomerang Attached");
        //set boomerang body parent to its parent holder.
        transform.SetParent(_parent.transform);
        //set position
        transform.localPosition = Vector3.zero;
        //stop movement
        Stop();
        //deactivate logic boomerang
        gameObject.SetActive(false);
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
            SlowDown();
    }

    private void FlyUninterrupted()
    {
        CalculateUninterruptedSpeed();
        Vector3 newVelocity = _reachedMaxDistance ?
            Vector3.Normalize(_parent.transform.position - transform.position) :
            _rb.velocity.normalized;

        newVelocity *= _currentSpeed;
        _rb.velocity = newVelocity;
    }

    private void CalculateUninterruptedSpeed()
    {
        float speedCurveModifier = _speedCurve.Evaluate(_distanceTravelled / _range);
        Mathf.Clamp(speedCurveModifier, 0.01f, 0.99f);
        if (_reachedMaxDistance)
            _currentSpeed = Vector3.Magnitude(_rb.velocity.normalized * _maxSpeed * (1 - speedCurveModifier));
        else
            _currentSpeed = Vector3.Magnitude(_rb.velocity.normalized * _maxSpeed * speedCurveModifier);
    }

    private void CalculateDistanceTravelled()
    {
        _distanceTravelled += _rb.velocity.magnitude * Time.fixedDeltaTime;
        _attachable = _distanceTravelled > _minDistanceToPickUp + 0.3f;
        _reachedMaxDistance = _distanceTravelled >= _range ? true : false;

        if (_reachedMaxDistance)
        {
            print("Reached Max Distance");
        }
    }

    private void CheckDamaging()
    {
        _damaging = _rb.velocity.magnitude > _minSpeedToDamage;
    }

    private void SlowDown()
    {
        if (_rb.velocity.magnitude <= 0.05f)
            _rb.velocity = Vector3.zero;
        else
            _rb.AddForce(-_rb.velocity.normalized * _slowDownForce * Time.fixedDeltaTime);
    }

    private void Stop()
    {
        _rb.velocity = Vector3.zero;
    }

}
