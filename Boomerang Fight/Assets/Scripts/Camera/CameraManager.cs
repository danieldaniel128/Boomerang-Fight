using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton
    static CameraManager instance;
    public static CameraManager Instance => instance;
    #endregion Singleton

    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] CameraShake cameraShake;

    public CameraShake CameraShakeRef => cameraShake;
    public CameraFollow CameraFollowRef => cameraFollow;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        #endregion Singleton
    }

    void Update()
    {
        cameraFollow.CameraFollowUpdate();
    }
}
