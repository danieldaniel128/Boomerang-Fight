using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _eliminatedFeed;//a ui that contains images and the nickname of the local player killed.
    [SerializeField] GameObject _eliminatorFeed;//a ui that contains images and nicknames of the one the killed and the one that was killed by the killed.
    [SerializeField] TMPro.TMP_Text _eliminatedNickname;
    [SerializeField] TMPro.TMP_Text _eliminatorNickname;

    [SerializeField] float _showEliminatedTime = 3;
    [SerializeField] float _showEliminatorTime = 3;

    public void ShowEliminatedFeed()
    {
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
}
