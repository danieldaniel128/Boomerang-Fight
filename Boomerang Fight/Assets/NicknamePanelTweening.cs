using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknamePanelTweening : MonoBehaviour
{
    public float timeUntilStart = 3f;
    public float tweenDuration = 0.7f;
    public Ease easyType = Ease.OutSine;
    public GameObject splashScreen;
    public List<RectTransform> objectsToTween = new();

    private List<Vector3> objectScales = new();
    float timer = 0f;
    bool tweened = false;

    private void Start()
    {
        SaveScales();
        //Tween();
    }

    private void Update()
    {
        if (tweened)
            return;

        timer += Time.deltaTime;

        if (timer > timeUntilStart)
        {
            splashScreen.SetActive(false);
            Tween();
        }
    }

    public void SaveScales()
    {
        for(int i = 0; i < objectsToTween.Count; i++)
        {
            objectScales.Add(objectsToTween[i].localScale);
        }

        foreach (var t in objectsToTween)
        {
            t.localScale = Vector3.zero;
        }
    }

    [ContextMenu("StartTween")]
    public void Tween()
    {
        ////make all scale 0
        //foreach (var t in objectsToTween)
        //{
        //    
        //}

        //tween to normal scale
        for (int i = 0; i < objectsToTween.Count; i++)
        {
            objectsToTween[i].DOScale(objectScales[i], tweenDuration).SetEase(easyType);
        }

        tweened = true;
    }

}
