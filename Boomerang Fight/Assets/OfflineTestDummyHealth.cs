using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OfflineTestDummyHealth : MonoBehaviour
{
    public UnityEvent<float, float> OnHealthChanged;
    public float maxHP;
    public float currentHP;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.gameObject.layer != 6)
            return;

        currentHP -= 1;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHealthChanged.Invoke(currentHP, maxHP);
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.layer != 6)
            return;

        currentHP -= 1;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        OnHealthChanged.Invoke(currentHP, maxHP);
    }

}
