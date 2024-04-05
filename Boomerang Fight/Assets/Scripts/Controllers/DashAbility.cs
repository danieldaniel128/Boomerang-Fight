using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    [Header("Dash Parameters")]
    [SerializeField] float _range;
    [SerializeField] float _duration;
    [SerializeField] float _cooldown;
    [SerializeField] AnimationCurve _speedCurve;
    [SerializeField] Rigidbody characterRB;

    public Action OnDash;
    public Action OnDashEnd;

    bool _inDash;
    bool _canDash = true;
    bool _onCooldown = false;
    Vector3 _forwardDirection;
    Vector3 _dashDestination;
    Vector3 _dashStartPosition;
    CountdownTimer _dashCooldown;
    CountdownTimer _dashDuration;

    //Vector3 DashDirection => _dashDestination - _dashStartPosition;

    public CountdownTimer DashDuration => _dashDuration;
    public bool InDash => _inDash;

    private void OnEnable()
    {
        SetTimers();
        _canDash = true;
    }

    private void Update()
    {
        TickTimers();
        if (_dashDuration.IsRunning)
            Dash();
    }

    private void OnDisable()
    {
        ClearTimers();
    }

    #region Timers

    private void TickTimers()
    {
        _dashCooldown.Tick(Time.deltaTime);
        _dashDuration.Tick(Time.deltaTime);
    }

    private void SetTimers()
    {
        //initialize everything to do with dash cooldown timer
        SetCooldownTimer();
        //initialize everything to do with dash duration timer
        SetDurationTimer();
    }

    private void ClearTimers()
    {
        ClearCooldownTimer();
        ClearDurationTimer();
    }

    #region DurationTimer
    private void SetDurationTimer()
    {
        _dashDuration = new(_duration);
        _dashDuration.OnTimerStart += StartDash;
        _dashDuration.OnTimerStop += EndDash;
    }

    private void ClearDurationTimer()
    {
        _dashDuration.OnTimerStart -= StartDash;
        _dashDuration.OnTimerStop -= EndDash;
    }
    #endregion DurationTimer

    #region CooldownTimer
    private void SetCooldownTimer()
    {
        _dashCooldown = new(_cooldown);
        _dashCooldown.OnTimerStart += StartCooldown;
        _dashCooldown.OnTimerStop += FinishCooldown;
    }

    private void ClearCooldownTimer()
    {
        _dashCooldown.OnTimerStart -= StartCooldown;
        _dashCooldown.OnTimerStop -= FinishCooldown;
    }

    #endregion CooldownTimer

    [ContextMenu("Reset Timers")]
    void ResetTimers()
    {
        _dashCooldown.Reset(_cooldown);
        _dashDuration.Reset(_duration);
    }

    #endregion Timers

    public void UpdateDashDirection(Vector3 newDirection)
    {
        _forwardDirection = newDirection;
        _forwardDirection.Normalize();
    }
    private void StartDash()
    {
        OnDash?.Invoke();
        _inDash = true;
    }
    private void EndDash()
    {
        _inDash = false;
        OnDashEnd?.Invoke();
    }

    private void FinishCooldown()
    {
        _onCooldown = false;
    }

    private void StartCooldown()
    {
        _onCooldown = true;
    }

    public void EnableDash(bool enabled)
    {
        _canDash = enabled;
    }

    //called from an external button
    [ContextMenu("TryDash")]
    public void TryStartDash()
    {
        if (_inDash)
            return;
        if (!_canDash)
            return;
        if (_onCooldown)
            return;

        //set dash destination
        _dashStartPosition = transform.position;
        _dashDestination = _dashStartPosition + _forwardDirection * _range;
        //start timers
        _dashCooldown.Start();
        _dashDuration.Start();

        OnDash?.Invoke();
    }

    private void Dash()
    {
        //lerp transform to facing direction + range
        float curveValue = _speedCurve.Evaluate(1 - _dashDuration.Progress);
        // update dash destination after checking collision?
        Vector3 newPosition = Vector3.Lerp(_dashStartPosition, _dashDestination, curveValue);

        characterRB.MovePosition(newPosition);
    }

    //call when hit a wall or death or something
    public void StopDash()
    {
        _dashDuration.Stop();
    }
}
