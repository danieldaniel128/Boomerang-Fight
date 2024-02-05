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
    Vector3 dashDestination;
    Vector3 dashStartPosition;
    CountdownTimer _dashCooldown;
    CountdownTimer _dashDuration;



    private void Start()
    {
        SetTimers();

        _canDash = true;
    }
    private void Update()
    {

        if (_dashCooldown.IsRunning)
        {
            _dashCooldown.Tick(Time.deltaTime);
        }

        if (_dashDuration.IsRunning)
        {
            _dashDuration.Tick(Time.deltaTime);
            Dash();
        }

    }

    

    private void SetTimers()
    {
        //initialize everything to do with dash cooldown timer
        SetCooldownTimer();
        //initialize everything to do with dash duration timer
        SetDurationTimer();
    }

    private void SetDurationTimer()
    {
        _dashDuration = new(_duration);
        _dashDuration.OnTimerStart += () => _inDash = true;
        _dashDuration.OnTimerStop += () => _inDash = false;
    }

    private void SetCooldownTimer()
    {
        _dashCooldown = new(_cooldown);
        _dashCooldown.OnTimerStart += () => _canDash = false;
        _dashCooldown.OnTimerStop += () => _canDash = true;
    }

    [ContextMenu("Reset Timers")]
    public void ResetTimers()
    {
        _dashCooldown.Reset(_cooldown);
        _dashDuration.Reset(_duration);
    }


    [ContextMenu("TryDash")]
    public void TryStartDash()
    {
        print("can dash: " + _canDash);
        if (_canDash)
        {
            //set dash destination
            dashStartPosition = transform.position;
            dashDestination = transform.position + transform.forward * _range;

            print("sPos: " + dashStartPosition);
            print("ePos: " + dashDestination);


            //start timers
            _dashCooldown.Start();
            _dashDuration.Start();
        }
    }

    void Dash()
    {
        //lerp transform to facing direction + range
        float curveValue = _speedCurve.Evaluate(1 - _dashDuration.Progress);
        Vector3 newPosition = Vector3.Lerp(dashStartPosition, dashDestination, curveValue);
        
        transform.position = newPosition;

    }

    //call when hit a wall or death or something
    public void StopDash()
    {
        _dashDuration.Stop();
    }
}
