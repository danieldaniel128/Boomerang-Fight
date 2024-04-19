using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
            other.attachedRigidbody.GetComponent<Health>().CallOnDeath();
    }
}
