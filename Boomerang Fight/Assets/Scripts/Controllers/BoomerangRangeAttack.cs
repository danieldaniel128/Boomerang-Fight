using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangRangeAttack : MonoBehaviourPun//interface of attacks
{
    public Action OnFinishRecalling { get; set; }
    [SerializeField] GameObject _boomerangParent;
    [SerializeField] GameObject _boomerangBody;
    [SerializeField] Transform _recallPos;
    [SerializeField] LayerMask _canAttackLayerMask;//seperate to base class.
    [Header("Boomerang Behaviors")]
    [SerializeField] AnimationCurve _attackSpeedCurve;
    [SerializeField] AnimationCurve _recallSpeedCurve;
    [Header("Boomerang Limits")]
    [SerializeField] float _maxBoomerangSpeed;
    [SerializeField] float _maxRecallBoomerangSpeed;
    [SerializeField] float _maxAttackRange;
    [Header("Components")]
    [SerializeField] Rigidbody _boomerangRigidbody;


    Vector3 _currentBoomerangFromPlayerVector =>  _recallPos.position - _boomerangBody.transform.position;
    private Vector3 _forwardDirection = Vector3.forward;
    private float _currentBoomerangFromPlayerDistance => _currentBoomerangFromPlayerVector.magnitude;
    private float _pressedRecallFromPlayerDistance;
    private bool _isFlying;//or can be called isAttacking.
    private bool _isSeperated;
    
    void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        if (!_isFlying && !_isSeperated)
            _boomerangRigidbody.velocity = Vector3.zero;
        FlyBoomerang();
    }
    private void Start()
    {
        if (!photonView.IsMine)
            return;
        _boomerangRigidbody.velocity = Vector3.zero;
    }
    private void FlyBoomerang()
    {
        if (!_isFlying)
            return;
        //set velocity of attacking. x is distance of player and released attack position, f(x) is velocity.
        Vector3 boomerangVelocity = _forwardDirection * _maxBoomerangSpeed * _attackSpeedCurve.Evaluate(_currentBoomerangFromPlayerDistance / _maxAttackRange);//_maxAttackRange cant be 0
        _boomerangRigidbody.velocity = boomerangVelocity; //attack time cant be 0
        if (_currentBoomerangFromPlayerDistance >= _maxAttackRange)
            StopBoomerang();
    }
    public void AttackRange(Vector3 attackDirection)
    {
        //cant use attack when is seperated.
        if (_isSeperated)
            return;
        ReleaseBoomerang();
        _forwardDirection = attackDirection;
        _isFlying = true;
    }
    public bool CanRecall()
    {
        //is not in hand and not flying?
        if (!_isFlying && _isSeperated)
            return true;
        return false;
    }
    public void Recall()
    {
        if (!photonView.IsMine)
            return;
        //set recall distance from boomerang.
        if (_boomerangRigidbody.velocity == Vector3.zero)
            _pressedRecallFromPlayerDistance = _currentBoomerangFromPlayerDistance;
        //set velocity of recalling. x is distance, f(x) is velocity.
        _boomerangRigidbody.velocity = _currentBoomerangFromPlayerVector.normalized * _maxRecallBoomerangSpeed * _recallSpeedCurve.Evaluate(_currentBoomerangFromPlayerDistance / _pressedRecallFromPlayerDistance);
        //checks if finished recalling.
        if(_currentBoomerangFromPlayerDistance <= 0.15f)
            AttachBoomerang();
    }
    private void StopBoomerang()
    {
        //stoped flying.
        _isFlying = false;
        //not on player.
        _isSeperated = true;
        //stop attacking velocity.
        _boomerangRigidbody.velocity = Vector3.zero;
        //fall on the floor.
        _boomerangRigidbody.useGravity = true;
    }
    private void ReleaseBoomerang()
    {
        //release boomerang from player prefab.
        transform.SetParent(null);
    }
    private void AttachBoomerang()
    {
        //finished recalling.
        OnFinishRecalling?.Invoke();
        //set boomerang body parent to its parent holder.
        transform.SetParent(_boomerangParent.transform);
        //set on player.
        _isSeperated = false;
        //stop recalling velocity.
        _boomerangRigidbody.velocity = Vector3.zero;
        //let it fly.
        _boomerangRigidbody.useGravity = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_canAttackLayerMask == (_canAttackLayerMask | (1 << collision.gameObject.layer)))
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(2);
        }
        StopBoomerang();
    }
}
