using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangRangeAttack : MonoBehaviour//interface of attacks
{

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
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlyBoomerang();
    }
    private void Update()
    {
    }
    [ContextMenu("attack test")]
    public void AttackMelee()
    {
        _isFlying = true;
    }
    private void FlyBoomerang()
    {
        if (!_isFlying)
            return;

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
    public void Recall()
    {
        if (_boomerangRigidbody.velocity == Vector3.zero)
            _pressedRecallFromPlayerDistance = _currentBoomerangFromPlayerDistance;
        //is not in hand and not flying?
        if (!_isFlying && _isSeperated)
        {
            //set speed of recalling x is distance, y is speed.
            _boomerangRigidbody.velocity = _currentBoomerangFromPlayerVector.normalized * _maxRecallBoomerangSpeed * _recallSpeedCurve.Evaluate(_currentBoomerangFromPlayerDistance / _pressedRecallFromPlayerDistance);
            //checks if finished recalling
            if(_currentBoomerangFromPlayerDistance <= 0.2f)
                AttachBoomerang();
            //Debug.Log("<color=red>Recalling</color>");
        }
    }
    private void StopBoomerang()
    {
        _isFlying = false;
        _boomerangRigidbody.velocity = Vector3.zero;
        _boomerangRigidbody.useGravity = true;
        _isSeperated = true;
    }
    private void ReleaseBoomerang()
    {
        _boomerangBody.transform.SetParent(null);
    }
    private void AttachBoomerang()
    {
        _boomerangBody.transform.SetParent(transform);
        _isSeperated = false;
        _boomerangRigidbody.velocity = Vector3.zero;
        _boomerangRigidbody.useGravity = false;
    }
}
