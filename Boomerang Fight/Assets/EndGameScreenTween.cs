using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScreenTween : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;

    private void OnEnable()
    {
        //set pos to screen height x2, 
        //tween screen to 0,0

        //rectTransform.rect.position = Vector2.up * rectTransform.rect.height;
        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;
        rectTransform.rect.Set(0, height, width, height);

        DOTween.To(() => height,
           x => {
               Vector2 size = rectTransform.sizeDelta;
               size.y = x;
               rectTransform.sizeDelta = size;
           },
           0, // Target height
           1.5f); // Duration of the animation
    }
}
