using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBoomerang : MonoBehaviour
{
    [SerializeField] GameObject _parent;

    [Header("Components")]
    [SerializeField] Rigidbody _rb;

    [Header("Boomerang Limits")]
    [SerializeField] float _slowDownForce;
    [SerializeField] float _minSpeedToDamage;
    [SerializeField] float _minDistanceToPickUp;


}
