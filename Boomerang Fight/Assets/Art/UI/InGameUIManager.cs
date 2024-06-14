using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIManager : MonoBehaviourPun
{
    [SerializeField] GameObject _eliminatedFeed;//a ui that contains images and the nickname of the local player killed.
    [SerializeField] GameObject _eliminatorFeed;//a ui that contains images and nicknames of the one the killed and the one that was killed by the killed.
    [SerializeField] TMPro.TMP_Text _eliminatedNickname;
    [SerializeField] TMPro.TMP_Text _eliminatorNickname;

    [SerializeField] float _showEliminatedTime = 3;
    [SerializeField] float _showEliminatorTime = 3;

    [Header("Controls UI")]
    [SerializeField] GameObject[] _ControlsUI;
    [SerializeField] GameObject _JoystickUI;
    [SerializeField] GameObject _dashUI;
    [SerializeField] GameObject _recallUI;

    [Header("On Death Screen")]
    [SerializeField] GameObject _deadScreenParent;
    [SerializeField] GameObject _RespawningCountdown;
    [SerializeField] TextMeshProUGUI _CountdownText;
    [SerializeField] float _countdownTweenScaleTime = 0.3f;

    public void ShowEliminatedFeed()
    {
        _eliminatedNickname.text = photonView.Owner.NickName;
        StartCoroutine(ShowFeedCoroutine(_eliminatedFeed, _showEliminatedTime));
    }
    public void ShowEliminatorFeed(string eliminatorNickname, string eliminatedNickname)
    {
        _eliminatorNickname.text = eliminatorNickname;
        _eliminatedNickname.text = eliminatedNickname;
        StartCoroutine(ShowFeedCoroutine(_eliminatorFeed, _showEliminatorTime));
    }

    private IEnumerator ShowFeedCoroutine(GameObject feedObject,float showFeedTime)
    {
        feedObject.SetActive(true);
        yield return new WaitForSeconds(showFeedTime);
        feedObject.SetActive(false);
    }

    #region Respawning

    public void PlayerDeath()
    {
        EnableDeathScreen();
        StartRespawningCountdown();
    }

    public void EnableDeathScreen()
    {
        _deadScreenParent.SetActive(true);

        _JoystickUI.SetActive(false);
        _dashUI.SetActive(false);
        _recallUI.SetActive(false);
    }

    public void DisableDeathScreen()
    {
        if (!photonView.IsMine)
            return;

        _deadScreenParent.SetActive(false);

        _JoystickUI.SetActive(true);
        _dashUI.SetActive(true);
    }

    public void StartRespawningCountdown()
    {
        StartCoroutine(RespawningCountdown());
    }

    void TweenCountDownText()
    {
        _CountdownText.transform.localScale = Vector3.zero;
        _CountdownText.transform.DOScale(1, _countdownTweenScaleTime).SetEase(Ease.OutSine);
    }

    private IEnumerator RespawningCountdown()
    {
        _CountdownText.text = "3";
        TweenCountDownText();
        yield return new WaitForSeconds(1);
        _CountdownText.text = "2";
        TweenCountDownText();
        yield return new WaitForSeconds(1);
        _CountdownText.text = "1";
        TweenCountDownText();
        yield return new WaitForSeconds(1);
        //call respawn player
        MultiplayerPlayerSpawner.Instance.TryRespawn(photonView.OwnerActorNr);
    }

    #endregion Respawning
}
