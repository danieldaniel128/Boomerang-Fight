using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimController : MonoBehaviour
{
    float range;

    public void SetRange(float r)
    {
        range = r;
    }

    public Vector3 GetNearestTarget()
    {
        if (TempLocalGameManager.Instance.PlayerCharacters.Count <= 1)
            return Vector3.zero;

        bool closePlayerFound = false;
        Vector3 closestTarget = Vector3.zero;
        float closestDistance = 0f;

        foreach (var player in TempLocalGameManager.Instance.PlayerCharacters)
        {
            if (player.photonView.IsMine)
                continue;

            if (!player.PlayerControllerRef.PlayerBody.activeInHierarchy)
                continue;

            float currentDistance = Vector3.Distance(player.transform.position, transform.position);

            if (currentDistance > range)
                continue;

            if (!closePlayerFound)
            {
                closePlayerFound = true;
                closestTarget = player.transform.position;
                closestDistance = currentDistance;
            }
            else
            {
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTarget = player.transform.position;
                }
            }
        }

        if (closePlayerFound)
            return closestTarget;
        else
            return Vector3.zero;

    }
}
