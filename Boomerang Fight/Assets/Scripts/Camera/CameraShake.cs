using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform camTransform;

    Vector3 originalPos;
    Quaternion originalRot;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        originalRot = camTransform.localRotation;
    }

    public void ShakeCamera(float duration = 0.6f, float amount = 0.5f)
    {
        camTransform.DOShakePosition(duration, amount).OnComplete(() => camTransform.localPosition = originalPos);
        camTransform.DOShakeRotation(duration, amount).OnComplete(() => camTransform.localRotation = originalRot);
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="duration"></param>
    ///// <param name="shakeAmount"></param>
    ///// <param name="decreaseFactor"></param>
    //public void ShakeCamera(float duration = 0.4f, float shakeAmount = 0.4f, float decreaseFactor = 1f)
    //{
    //    this.shakeAmount = shakeAmount;
    //    this.shakeDuration = duration;
    //    this.decreaseFactor = decreaseFactor;
    //}

    //public void CameraShakeUpdate()
    //{
    //    if (shakeDuration > 0)
    //    {
    //        camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

    //        shakeDuration -= Time.deltaTime * decreaseFactor;
    //    }
    //    else
    //    {
    //        shakeDuration = 0f;
    //        camTransform.localPosition = originalPos;
    //    }
    //}




    [SerializeField] float testDuration = 0.4f;
    [SerializeField] float testAmount = 0.4f;

    [ContextMenu("Test Tween Shake")]
    void TestShakeCamera()
    {
        ShakeCamera(testDuration, testAmount);
    }

}
