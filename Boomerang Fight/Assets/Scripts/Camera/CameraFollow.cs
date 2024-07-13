using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float smoothTime = 0.3f;

    [SerializeField] private Vector3 followOffset;
    private Vector3 velocity = Vector3.zero;
    private float startingSmoothTime;
    private float elapsedTime = 0f;
    Vector3 startPos;
    bool startFollow = true;
    private void Awake()
    {
        startingSmoothTime = smoothTime;
        startPos = transform.position;
    }
    public void FollowTarget()
    {
        if (startFollow)
            StartCameraFollow();
        else
            CameraFollowUpdate();
    }
    public void CameraFollowUpdate()
    {
        if (target == null)
            return;
        // Define a target position above and behind the target transform
        Vector3 targetPosition = target.position + followOffset;

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    }
    public void StartCameraFollow()
    {
        if (target == null)
            return;
        Vector3 targetPosition = target.position + followOffset;
        elapsedTime += Time.fixedDeltaTime;
        float t = elapsedTime / smoothTime;
        t = Mathf.Clamp01(t); // Ensure t stays between 0 and 1
        // Linearly interpolate the camera's position
        transform.position = Vector3.Lerp(startPos, targetPosition, t);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void SetFollowTime(float newFollowTime)
    {
        smoothTime = newFollowTime;
        StartCoroutine(WaitForFinishCameraSetUP());
    }
    public void InitSmoothTime()
    {
        smoothTime = startingSmoothTime;
    }
    IEnumerator WaitForFinishCameraSetUP()
    {
        yield return new WaitForSeconds(3);
        InitSmoothTime();
        startFollow = false;
    }

}
