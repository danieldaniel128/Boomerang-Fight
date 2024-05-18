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
            playerHealth = other.attachedRigidbody.GetComponent<Health>();
        //making sure the player dies for good in case we have no time for respawn
            playerHealth?.CallOnDeath();
            playerHealth?.CallOnDeath();
            playerHealth?.CallOnDeath();
            playerHealth?.CallOnDeath();
            playerHealth?.CallOnDeath();
            playerHealth?.CallOnDeath();
        if(playerHealth == null)
            return;
        if (playerHealth.IsDead)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(0);
        }
    }
}
