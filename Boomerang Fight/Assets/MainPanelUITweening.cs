using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanelUITweening : MonoBehaviour
{
    [SerializeField] RectTransform[] UpTransforms;
    [SerializeField] RectTransform[] DownTransforms;
    [SerializeField] RectTransform[] LeftTransforms;
    [SerializeField] RectTransform[] RightTransforms;
    [SerializeField] float minTweenTime = 0.2f;
    [SerializeField] float maxTweenTime = 0.5f;
    [SerializeField] float outOfScreenDistance = 200;
    [SerializeField] Ease easingType = Ease.OutElastic;
    float RandomTweenTime => Random.Range(minTweenTime, maxTweenTime);
    float RandomBounceOverShootAmount => Random.Range(0f, 1f);

    private void OnEnable()
    {
        TweenTransformsOnScreen();
    }

    [ContextMenu("Tween UI")]
    void TweenTransformsOnScreen()
    {
        TweenUpUI();
        TweenDownUI();
        TweenLeftUI();
        TweenRightUI();
    }

    private void TweenUpUI()
    {
        foreach (RectTransform t in UpTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            t.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y + outOfScreenDistance);
            t.DOAnchorPos(initPos, RandomTweenTime).SetEase(easingType);
        }
    }
    private void TweenDownUI()
    {
        foreach (RectTransform t in DownTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            t.anchoredPosition = new Vector2(t.anchoredPosition.x, t.anchoredPosition.y - outOfScreenDistance);
            t.DOAnchorPos(initPos, RandomTweenTime).SetEase(easingType);
        }
    }
    private void TweenLeftUI()
    {
        foreach (RectTransform t in LeftTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            t.anchoredPosition = new Vector2(t.anchoredPosition.x - outOfScreenDistance, t.anchoredPosition.y);
            t.DOAnchorPos(initPos, RandomTweenTime).SetEase(easingType);
        }
    }
    private void TweenRightUI()
    {
        foreach (RectTransform t in RightTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            t.anchoredPosition = new Vector2(t.anchoredPosition.x + outOfScreenDistance, t.anchoredPosition.y);
            t.DOAnchorPos(initPos, RandomTweenTime).SetEase(easingType);
        }
    }
}
