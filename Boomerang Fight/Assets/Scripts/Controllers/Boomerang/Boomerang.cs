using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] Rigidbody _rb;

    [Space]
    [SerializeField] GameObject _parent;
    [SerializeField] float _slowDownForce;
    [SerializeField] float _minSpeedToDamage;
    [SerializeField] float _minDistanceToPickUp;

    public Rigidbody RB { get { return _rb; } }
    public GameObject Parent { get { return _parent; } }
    public bool IsFlying { get => _isFlying; set => _isFlying = value; }
    public bool IsSeperated { get => _isSeperated; set => _isSeperated = value; }
    public bool Interrupted { get => _interupted; set => _interupted = value; }
    public bool ReachedMaxRange { get => _reachedMaxRange; set => _reachedMaxRange = value; }
    public float Damage { get => _damage; }
    public float MinDistanceToPickUp { get => _minDistanceToPickUp;}

    protected bool _isFlying;
    protected bool _isSeperated;
    private bool _interupted; // to check if hit anything that changes its trajectory
    private bool _reachedMaxRange; // to check if reached max range and should start returning to player
    LayerMask _canAttackLayerMask;
    float _damage;

    private void Update()
    {
        if (!IsFlying && IsSeperated)
        {
            if (RB.velocity.magnitude <= 0.1f)
            {
                RB.velocity = Vector3.zero;
            }

            RB.AddForce(-RB.velocity.normalized * _slowDownForce);
            if(Vector3.Distance(transform.position, _parent.transform.position) < MinDistanceToPickUp)
            {
                Attach();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (RB.velocity.magnitude <= _minSpeedToDamage)
            return;

        if (_canAttackLayerMask == (_canAttackLayerMask | (1 << other.gameObject.layer)))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(Damage);
        }
    }

    public void SetBoomerangDamage(float damage)
    {
        _damage = damage;
    }
    public void SetAttackLayerMask(LayerMask layerMask)
    {
        _canAttackLayerMask = layerMask;
    }

    //release from player prefab.
    public void Release()
    {
        print("Boomerang Released");
        transform.SetParent(null);
        _isSeperated = true;
        _reachedMaxRange = false;
        _interupted = false;
    }
    //attach to player prefab
    public void Attach()
    {
        print("Boomerang Attached");
        //set boomerang body parent to its parent holder.
        transform.SetParent(_parent.transform);
        //set position
        transform.localPosition = Vector3.zero;
        //set on player.
        _isSeperated = false;
        //stop movement
        Stop();
        //ignore gravity
        _rb.useGravity = false;
    }
    public void Stop()
    {
        //stoped flying.
        IsFlying = false;
        //stop velocity.
        RB.velocity = Vector3.zero;
    }

}
