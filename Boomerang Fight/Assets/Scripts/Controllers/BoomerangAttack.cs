using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangAttack : MonoBehaviour//interface of attacks
{

    [SerializeField] GameObject _boomerangBody;
    [Header("Boomerang Behaviors")]
    [SerializeField] AnimationCurve _attackSpeedCurve;
    [SerializeField] AnimationCurve _recallSpeedCurve;
    [Header("Boomerang Limits")]
    [SerializeField] float _maxBoomerangSpeed;
    [SerializeField] float _maxRecallBoomerangSpeed;
    [SerializeField] float attackTime;
    [Header("Components")]
    [SerializeField] Rigidbody _boomerangRigidbody;

    private bool isFlying;//or can be called isAttacking.
    private bool isSeperated;
    private Vector3 _forwardDirection = Vector3.forward;
    private CountdownTimer _attackTimer;
    float _pressedRecallFromPlayerDistance;
    
    private void OnEnable()
    {
        _attackTimer = new CountdownTimer(attackTime);
        _attackTimer.OnTimerStop += StopBoomerang;
    }
    private void OnDisable()
    {
        _attackTimer.OnTimerStop -= StopBoomerang;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlyBoomerang();
    }
    private void Update()
    {
        _attackTimer.Tick(Time.deltaTime);
    }
    [ContextMenu("attack test")]
    public void AttackMelee()
    {
        _attackTimer.Start();
        isFlying = true;
    }
    private void FlyBoomerang()
    {
        if (!isFlying)
            return;
        Vector3 boomerangVelocity = _forwardDirection * _maxBoomerangSpeed * _attackSpeedCurve.Evaluate(1 - _attackTimer.Progress);
        _boomerangRigidbody.velocity = boomerangVelocity; //attack time cant be 0
    }
    public void AttackRange(Vector3 attackDirection)
    {
        //cant use attack when is seperated.
        if (isSeperated)
            return;
        ReleaseBoomerang();
        _attackTimer.Restart();
        _forwardDirection = attackDirection;
        isFlying = true;
    }
    public void Recall(Transform origin)
    {
        if (_boomerangRigidbody.velocity == Vector3.zero)
            _pressedRecallFromPlayerDistance = Vector3.Distance(origin.position, _boomerangBody.transform.position);
        //is not in hand and not flying?
        if (!isFlying && isSeperated)
        {
            //vector of player pos to boomerang pos.
            Vector3 boomerangToPlayerVector = origin.position - _boomerangBody.transform.position;
            //direction of boomerang to player.
            Vector3 playerDirection = boomerangToPlayerVector.normalized;
            //remaining distance of boomerang and player.
            float playerDistance = boomerangToPlayerVector.magnitude;
            //set speed of recalling x is distance, y is speed.
            _boomerangRigidbody.velocity = playerDirection * _maxRecallBoomerangSpeed * _recallSpeedCurve.Evaluate(playerDistance/ _pressedRecallFromPlayerDistance);
            //checks if finished recalling
            if(playerDistance <= 0.2f)
                AttachBoomerang();
        }
    }
    private void StopBoomerang()
    {
        isFlying = false;
        _boomerangRigidbody.velocity = Vector3.zero;
        _boomerangRigidbody.useGravity = true;
        isSeperated = true;
        _attackTimer.Reset();
    }
    private void ReleaseBoomerang()
    {
        _boomerangBody.transform.SetParent(null);
    }
    private void AttachBoomerang()
    {
        _boomerangBody.transform.SetParent(transform);
        isSeperated = false;
        _boomerangRigidbody.velocity = Vector3.zero;
        _boomerangRigidbody.useGravity = false;
    }
}
