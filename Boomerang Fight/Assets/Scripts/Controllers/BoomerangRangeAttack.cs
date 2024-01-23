using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangRangeAttack : MonoBehaviour//interface of attacks
{
    public Action OnFinishRecalling { get; set; }

    [SerializeField] GameObject _boomerangBody;
    [SerializeField] Transform _recallPos;
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
        FlyBoomerang();
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
        //set recall distance from boomerang.
        if (_boomerangRigidbody.velocity == Vector3.zero)
            _pressedRecallFromPlayerDistance = _currentBoomerangFromPlayerDistance;
        //set velocity of recalling. x is distance, f(x) is velocity.
        _boomerangRigidbody.velocity = _currentBoomerangFromPlayerVector.normalized * _maxRecallBoomerangSpeed * _recallSpeedCurve.Evaluate(_currentBoomerangFromPlayerDistance / _pressedRecallFromPlayerDistance);
        //checks if finished recalling.
        if(_currentBoomerangFromPlayerDistance <= 0.2f)
            AttachBoomerang();
        //Debug.Log("<color=red>Recalling</color>");
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
        _boomerangBody.transform.SetParent(null);
    }
    private void AttachBoomerang()
    {
        //finished recalling.
        StartCoroutine(FinishRecalling());
        //set boomerang body parent to this.
        _boomerangBody.transform.SetParent(transform);
        //set on player.
        _isSeperated = false;
        //stop recalling velocity.
        _boomerangRigidbody.velocity = Vector3.zero;
        //let it fly.
        _boomerangRigidbody.useGravity = false;
    }
    private IEnumerator FinishRecalling()
    {
        yield return new WaitForEndOfFrame();
        //finished recalling.
        OnFinishRecalling?.Invoke();
    }
}
