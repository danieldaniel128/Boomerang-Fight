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
    [SerializeField] float minTweenTime = .7f;
    [SerializeField] float maxTweenTime = .7f;
    [SerializeField] float outOfScreenDistance = 250;
    [SerializeField] Ease easingType = Ease.OutSine;

    float RandomTweenTime => Random.Range(minTweenTime, maxTweenTime);
    float RandomBounceOverShootAmount => Random.Range(0f, 1f);

    Dictionary<RectTransform, Vector2> transformInitialPosDictionary = new();


    private void OnEnable()
    {
        transformInitialPosDictionary.Clear();
        SetTransformOffScreen(TweenDirection.Up);
        SetTransformOffScreen(TweenDirection.Right);
        SetTransformOffScreen(TweenDirection.Left);
        SetTransformOffScreen(TweenDirection.Down);

        StartCoroutine(TweenTransformsOnScreen());
    }


    IEnumerator TweenTransformsOnScreen()
    {
        //List<Sequence> sequences = new List<Sequence>();
        //sequences.Add(TweenDirectionalUI(TweenDirection.Right));
        //sequences.Add(TweenDirectionalUI(TweenDirection.Up));
        //sequences.Add(TweenDirectionalUI(TweenDirection.Down));
        //sequences.Add(TweenDirectionalUI(TweenDirection.Left));

        //sequences[0].Play().SetEase(easingType);
        //yield return new WaitForSecondsRealtime(1f);
        //sequences[1].Play().SetEase(easingType);
        ////yield return new WaitForSeconds(0.1f);
        //sequences[2].Play().SetEase(easingType);
        //yield return new WaitForSecondsRealtime(1f);
        //sequences[3].Play().SetEase(easingType);



        TweenTransformOnScreen(TweenDirection.Up);
        yield return new WaitForSecondsRealtime(0.15f);
        TweenTransformOnScreen(TweenDirection.Right);
        TweenTransformOnScreen(TweenDirection.Left);
        yield return new WaitForSecondsRealtime(0.15f);
        TweenTransformOnScreen(TweenDirection.Down);

    }

    void SetTransformOffScreen(TweenDirection dir)
    {
        RectTransform[] rectTransforms = GetTransformsFromDirection(dir);
        Vector2 offScreenDirection = GetOffScreenDirectionVector(dir);
        foreach (RectTransform t in rectTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            transformInitialPosDictionary.Add(t, initPos);
            t.anchoredPosition += offScreenDirection;
            //t.DOAnchorPos(initPos, RandomTweenTime).SetEase(easingType);
        }
    }

    void TweenTransformOnScreen(TweenDirection dir)
    {
        RectTransform[] rectTransforms;
        switch (dir)
        {
            case TweenDirection.Up:
                rectTransforms = UpTransforms;
                break;
            case TweenDirection.Right:
                rectTransforms = RightTransforms;
                break;
            case TweenDirection.Left:
                rectTransforms = LeftTransforms;
                break;
            case TweenDirection.Down:
                rectTransforms = DownTransforms;
                break;
            default:
                return;
        }

        foreach(var t in rectTransforms)
        {
            Vector2 pos;
            transformInitialPosDictionary.TryGetValue(t, out pos);
            t.DOAnchorPos(pos, RandomTweenTime).SetEase(easingType);
        }
    }

    Sequence TweenDirectionalUI(TweenDirection dir)
    {
        var sequence = DOTween.Sequence();

        RectTransform[] rectTransforms = GetTransformsFromDirection(dir);
        Vector2 offScreenDirection = GetOffScreenDirectionVector(dir);
        foreach (RectTransform t in rectTransforms)
        {
            Vector2 initPos = t.anchoredPosition;
            t.anchoredPosition += offScreenDirection;
            sequence.Join(t.DOAnchorPos(initPos, RandomTweenTime));
        }
        return sequence;
    }
    

    Vector2 GetOffScreenDirectionVector(TweenDirection dir)
    {
        switch (dir)
        {
            case TweenDirection.Up:
                return Vector2.up * outOfScreenDistance;
            case TweenDirection.Right:
                return Vector2.right * outOfScreenDistance;
            case TweenDirection.Left:
                return Vector2.left * outOfScreenDistance;
            case TweenDirection.Down:
                return Vector2.down * outOfScreenDistance;
            default:
                return Vector2.zero;
        }
    }


    RectTransform[] GetTransformsFromDirection(TweenDirection dir)
    {
        switch (dir)
        {
            case TweenDirection.Up:
                return UpTransforms;
            case TweenDirection.Right:
                return RightTransforms;
            case TweenDirection.Left:
                return LeftTransforms;
            case TweenDirection.Down:
                return DownTransforms;
            default: return null;
        }
    }

    enum TweenDirection
    {
        Up,
        Right,
        Left,
        Down
    }
}
