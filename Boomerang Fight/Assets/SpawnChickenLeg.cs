using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChickenLeg : MonoBehaviour
{
    public GameObject chickenLegPrefab;
    public int amountOfLegs = 2;
    public float minForce = 2f;
    public float maxForce = 4f;

    private void OnDisable()
    {
        for (int i = 0; i < amountOfLegs; i++)
        {
            GameObject leg = Instantiate(chickenLegPrefab, transform.position, Quaternion.identity);
            Rigidbody rb = leg.GetComponent<Rigidbody>();
            //leg.GetComponent<Rigidbody>().AddForce
            Vector3 randomUpwards = Vector3.up + new Vector3(Random.Range(-0.2f, 0.2f), 0f, Random.Range(-0.2f, 0.2f));
            rb.AddForce(randomUpwards.normalized * Random.Range(minForce, maxForce), ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            rb.AddTorque(randomTorque * Random.Range(1, 2), ForceMode.Impulse);
        }
    }
}
