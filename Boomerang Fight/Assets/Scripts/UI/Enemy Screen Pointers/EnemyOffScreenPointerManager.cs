using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOffScreenPointerManager : MonoBehaviourPun
{
    [SerializeField] EnemyOffScreenPointer[] pointers;
    [SerializeField] Transform playerTransform;

    bool initialized = false;

    private void Awake()
    {
        if (photonView.IsMine)
            MultiplayerPlayerSpawner.Instance.OnAllPlayersJoined += InitializePointers;
    }


    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (!initialized)
            return;

        foreach (EnemyOffScreenPointer pointer in pointers)
        {
            if (!pointer.gameObject.activeInHierarchy)
                continue;

            //if player body is off, deactivate pointer.
            if (!pointer.TargetPlayer.PlayerControllerRef.PlayerBody.activeInHierarchy)
            {
                pointer.DeactivatePointer();
                continue;
            }

            pointer.UpdatePointer();
        }
    }
    //void InitializePointers()
    //{
    //    int index = 0;
    //    print("amount of playerCharacters on initializing pointers: " + TempLocalGameManager.Instance.PlayerCharacters.Count);
    //    foreach (var player in TempLocalGameManager.Instance.PlayerCharacters)
    //    {
    //        if (player.photonView.IsMine)
    //            continue;
    //        pointers[index].ActivatePointer();
    //        pointers[index].Initialize(playerTransform, player);
    //        index++;
    //    }
    //    initialized = true;
    //}

    void InitializePointers()
    {
        if (photonView.IsMine)
            StartCoroutine(InitializePointersWithDelay());
    }

    IEnumerator InitializePointersWithDelay()
    {
        yield return new WaitForSeconds(2f);
        int index = 0;
        print("amount of playerCharacters on initializing pointers: " + TempLocalGameManager.Instance.PlayerCharacters.Count);
        foreach (var player in TempLocalGameManager.Instance.PlayerCharacters)
        {
            if (player.photonView.IsMine)
                continue;

            pointers[index].ActivatePointer();
            pointers[index].Initialize(playerTransform, player);
            index++;
        }
        initialized = true;
    }

}
