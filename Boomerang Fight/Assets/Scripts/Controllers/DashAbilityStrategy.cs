using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbilityStrategy : MonoBehaviour
{
    [Header("Dash Parameters")]
    [SerializeField] float _range;
    [SerializeField] float _duration;
    [SerializeField] float _cooldown;
    [SerializeField] AnimationCurve _speedCurve;


    bool _inDash;
    bool _canDash = true;
    Vector3 _dashDestination;
    Vector3 _dashStartPosition;
    CountdownTimer _dashCooldown;
    CountdownTimer _dashDuration;


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
        _dashDuration.OnTimerStart += ActivateDash;
        _dashDuration.OnTimerStop += DeactivateDash;
    }

    private void ActivateDash()
    {
        _inDash = true;
    }

    private void DeactivateDash()
    {
        _inDash = false;
    }

    private void ClearDurationTimer()
    {
        _dashDuration.OnTimerStart -= ActivateDash;
        _dashDuration.OnTimerStop -= DeactivateDash;
    }
    #endregion DurationTimer

    #region CooldownTimer
    private void SetCooldownTimer()
    {
        _dashCooldown = new(_cooldown);
        _dashCooldown.OnTimerStart += DisableDash;
        _dashCooldown.OnTimerStop += EnableDash;
    }

    private void EnableDash()
    {
        _canDash = true;
    }

    private void DisableDash()
    {
        _canDash = false;
    }

    private void ClearCooldownTimer()
    {
        _dashCooldown.OnTimerStart -= DisableDash;
        _dashCooldown.OnTimerStop -= EnableDash;
    }

    #endregion CooldownTimer

    [ContextMenu("Reset Timers")]
    void ResetTimers()
    {
        _dashCooldown.Reset(_cooldown);
        _dashDuration.Reset(_duration);
    }

    #endregion Timers


    [ContextMenu("TryDash")]
    public void TryStartDash()
    {
        print("can dash: " + _canDash);
        if (_canDash)
        {
            //set dash destination
            _dashStartPosition = transform.position;
            _dashDestination = _dashStartPosition + transform.forward * _range;
            //start timers
            _dashCooldown.Start();
            _dashDuration.Start();
        }
    }

    private void Dash()
    {
        //lerp transform to facing direction + range
        float curveValue = _speedCurve.Evaluate(1 - _dashDuration.Progress);
        Vector3 newPosition = Vector3.Lerp(_dashStartPosition, _dashDestination, curveValue);
        transform.position = newPosition;
    }

    //call when hit a wall or death or something
    public void StopDash()
    {
        _dashDuration.Stop();
    }
}
