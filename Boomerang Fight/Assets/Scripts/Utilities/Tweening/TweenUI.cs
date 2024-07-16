using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] TweenUISettings settings;

    private void Awake()
    {
        if (settings.tweenType == TweenType.Position)
        {
            Vector2 pos = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y);
            rectTransform.anchoredPosition = settings.ReplaceStartingPosition(pos);
        }
       
        if(settings.tweenType == TweenType.Image)
        {
            settings.SaveStartingOpacity();
        }

    }

    private void OnEnable()
    {
        if (settings.startType == TweenStartType.Enabled)
            settings.StartTween();
    }

    public void ActivateTween()
    {
        settings.StartTween();
    }
}
