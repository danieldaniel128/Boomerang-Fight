using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health playerHealth = null;
        if (other.attachedRigidbody.GetComponent<PhotonView>().IsMine)
        {
            playerHealth = other.attachedRigidbody.GetComponent<Health>();
        }
    }
}
