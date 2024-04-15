using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VFXTransitioner : MonoBehaviourPun
{
    [SerializeField] VFXTransition[] _vFXTransitions;
    [SerializeField] float _playTriggerVFXTime;
    public void ActivateVFX(VFXTypeEnum vFXType)
    {
        for (int i = 0; i < _vFXTransitions.Length; i++)
        {
            if (_vFXTransitions[i].VFXType == vFXType)
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
    }
    public void DeActivateProlongedVFX(VFXTypeEnum vFXType)
    {
        for (int i = 0; i < _vFXTransitions.Length; i++)
        {
            if (_vFXTransitions[i].VFXType == vFXType)
                photonView.RPC(nameof(SyncDeActivateProlongedVFX), RpcTarget.All, i);
        }
    }
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


