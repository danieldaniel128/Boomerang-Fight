using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.VisualScripting;

[Serializable]
public class TweenUISettings
{
    [SerializeField] public TweenStartType startType;
    [SerializeField] public TweenType tweenType;

    [Header("Components")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image image;

    [Header("Target Result")]
    [SerializeField] Vector2 tweenStartPosition;
    [SerializeField] float opacity;

    [Header("tweening parameters")]
    [SerializeField] float duration;
    [SerializeField] Ease easeType;
    [SerializeField] int loopAmount;
    [SerializeField] LoopType loopType;

    [Header("Events")]
    public UnityEvent OnTweenCompleted;

    Tween currentTween;
    Vector2 startingAnchoredPos;
    float startingOpacity;

    public Vector2 ReplaceStartingPosition(Vector2 pos)
    {
        startingAnchoredPos = new Vector2(pos.x, pos.y);
        return tweenStartPosition;
    }

    public void SaveStartingOpacity()
    {
        startingOpacity = image.color.a;
    }

    public void StartTween()
    {
        switch (tweenType)
        {
            case TweenType.Position:
                currentTween = rectTransform.DOAnchorPos(startingAnchoredPos, duration).Pause();
                break;
            case TweenType.Image:
                image.color = new Color(1, 1, 1, startingOpacity);
                currentTween = image.DOFade(opacity, duration).Pause();
                break;
            default:
                break;
        }

        currentTween.SetEase(easeType).SetLoops(loopAmount, loopType).OnComplete(() => OnTweenCompleted.Invoke());
        currentTween.Play();
    }
}

public enum TweenType
{
    Position,
    Image
}

public enum TweenStartType
{
    Enabled,
    Event
}