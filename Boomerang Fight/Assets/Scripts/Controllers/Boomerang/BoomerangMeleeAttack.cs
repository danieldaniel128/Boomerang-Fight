using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class BoomerangMeleeAttack : MonoBehaviour
{
    public Action OnAttackPressed { get; set; }

    [SerializeField] bool showGizmos;

    [Header("Size Parameters")]
    [SerializeField] float colliderWidth;
    [SerializeField] float colliderDepth;
    [SerializeField] float colliderHeight;
    [SerializeField] float distanceFromPlayer;

    [Header("Timing Parameters")]
    [SerializeField] float timeToStartAttack;
    [SerializeField] float attackDuration;
    [SerializeField] float cooldownDuration;

    bool canAttack = true;
    bool inAttack = false;
    CountdownTimer cooldownTimer;
    CountdownTimer delayToAttackTimer;

    private void Start()
    {
        OnAttackPressed += TryInitiateAttack;

        //set up cooldown timer
        cooldownTimer = new(cooldownDuration);
        cooldownTimer.OnTimerStop += () => canAttack = true;

        //set up delay to attack timer
        delayToAttackTimer = new(timeToStartAttack);
        delayToAttackTimer.OnTimerStop += Attack;
    }
    private void FixedUpdate()
    {
        Tick();
    }

    void Tick()
    {
        delayToAttackTimer.Tick(Time.fixedDeltaTime);
        cooldownTimer.Tick(Time.fixedDeltaTime);
    }

    [ContextMenu("Press Attack")]
    public void ActivateAttackEvent()
    {
        print("Attack invoked");
        OnAttackPressed?.Invoke();
    }

    public void TryInitiateAttack()
    {
        if (canAttack)
        {
            canAttack = false;

            delayToAttackTimer.Start();
            cooldownTimer.Start();
        }

    }

    public void Attack()
    {
        Collider[] colliders = GetCollidersInAttackRange();

        foreach (Collider target in colliders)
        {
            //add filter for what gets hit
            HitTarget(target);
        }
    }

    public void HitTarget(Collider target)
    {
        print("hit target: " + target.name);
    }

    [ContextMenu("Activate Attack Collider")]
    public Collider[] GetCollidersInAttackRange()
    {
        //collider size
        Vector3 colliderSize = new(colliderWidth, colliderHeight, colliderDepth);

        //convert from local space to world space for physics overlap box
        Vector3 localBoxCenter = Vector3.forward * distanceFromPlayer;
        Vector3 worldBoxCenter = transform.TransformPoint(localBoxCenter);

        Collider[] collidersHit = Physics.OverlapBox(worldBoxCenter, colliderSize / 2, transform.rotation);
        return collidersHit;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        if (canAttack)
            Gizmos.color = Color.green;
        else
        {
            if (inAttack)
                Gizmos.color = Color.blue;
            else
                Gizmos.color = Color.red;
        }

        //convert from local to world space for draw wire cube
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Vector3 colliderSize = new(colliderWidth, colliderHeight, colliderDepth);
        Gizmos.DrawWireCube(Vector3.forward * distanceFromPlayer, colliderSize);
    }
}
