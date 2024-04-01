using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 followOffset;
    private Vector3 velocity = Vector3.zero;

    public void CameraFollowUpdate()
    {
        if (target == null)
            return;
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.position + followOffset;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

}
