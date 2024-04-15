using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXTransitioner : MonoBehaviourPun
{
    [SerializeField] VFXTransition[] _vFXTransitions;
    [SerializeField] float _playTriggerVFXTime;
    public void ActivateVFX(VFXTypeEnum vFXType, bool local = false)
    {
        for (int i = 0; i < _vFXTransitions.Length; i++)
        {
            if (_vFXTransitions[i].VFXType != vFXType)
                continue;

            if (!local)
            {
                if (_vFXTransitions[i].IsTriggerVFX)
                {
                    photonView.RPC(nameof(SyncTriggerVFX), RpcTarget.All, i);
                    break;
                }
                else
                {
                    photonView.RPC(nameof(SyncProlongedVFX), RpcTarget.All, i);
                    break;
                }
            }
            else
            {
                if (_vFXTransitions[i].IsTriggerVFX)
                    TriggerVFX(i);
                else
                    ProlongedVFX(i);
            }
        }
    }
    public void DeActivateProlongedVFX(VFXTypeEnum vFXType, bool local = false)
    {
        for (int i = 0; i < _vFXTransitions.Length; i++)
        {
            if (_vFXTransitions[i].VFXType != vFXType)
                continue;

            if (local)
                _vFXTransitions[i].gameObject.SetActive(false);
            else
                photonView.RPC(nameof(SyncDeActivateProlongedVFX), RpcTarget.All, i);
        }
    }
    #region Local
    private void TriggerVFX(int vfxIndex)
    {
        _vFXTransitions[vfxIndex].gameObject.SetActive(true);
        StartCoroutine(FinishTriggerCoroutine(_vFXTransitions[vfxIndex].gameObject));
    }
    private void ProlongedVFX(int vfxIndex)
    {
        _vFXTransitions[vfxIndex].gameObject.SetActive(true);
    }
    #endregion Local

    [PunRPC]
    private void SyncTriggerVFX(int vfxIndex)
    {
        _vFXTransitions[vfxIndex].gameObject.SetActive(true);
        StartCoroutine(FinishTriggerCoroutine(_vFXTransitions[vfxIndex].gameObject));
    }
    [PunRPC]
    private void SyncProlongedVFX(int vfxIndex)
    {
        _vFXTransitions[vfxIndex].gameObject.SetActive(true);
    }
    [PunRPC]
    private void SyncDeActivateProlongedVFX(int vfxIndex)
    {
        _vFXTransitions[vfxIndex].gameObject.SetActive(false);
    }
    private IEnumerator FinishTriggerCoroutine(GameObject vfxGameObject)
    {
        yield return new WaitForSeconds(_playTriggerVFXTime);
        vfxGameObject.SetActive(false);
    }

}


