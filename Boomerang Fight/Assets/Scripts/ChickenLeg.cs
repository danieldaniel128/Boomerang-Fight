using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenLeg : MonoBehaviour
{
    public float duration = 5f;
    public LayerMask enemyLayer;
    private void Start()
    {
        Invoke(nameof(DestroyObject), duration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
