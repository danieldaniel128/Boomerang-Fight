using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashUIController : MonoBehaviour
{
    [Header("Flash On Hit UI")]
    [SerializeField] List<Image> _ImagesToFlash = new();
    [SerializeField] List<SpriteRenderer> _SpritesToFlash = new();
    [SerializeField] float _flashDuration = 0.7f;
    [SerializeField] float _peakFlashAmount = 0.8f;
    private void OnEnable()
    {
        InitializeFlashMaterial();
    }

    #region FlashOnHit

    [ContextMenu("flashUI")]
    public void FlashUI() //called in OnHit event
    {
        foreach (var i in _ImagesToFlash)
        {
            i.material.SetFloat("_FlashAmount", _peakFlashAmount);
            i.material.DOFloat(0f, "_FlashAmount", _flashDuration).SetEase(Ease.InSine);
        }
        foreach (var s in _SpritesToFlash)
        {
            s.material.SetFloat("_FlashAmount", _peakFlashAmount);
            s.material.DOFloat(0f, "_FlashAmount", _flashDuration).SetEase(Ease.InSine);
        }
    }

    public void ResetUIFlash()
    {
        foreach (var i in _ImagesToFlash)
            i.material.SetFloat("_FlashAmount", 0f);
        foreach (var s in _SpritesToFlash)
            s.material.SetFloat("_FlashAmount", 0f);
    }

    public void InitializeFlashMaterial()
    {
        foreach (var image in _ImagesToFlash)
        {
            image.material = new Material(image.material);
        }
        foreach (var s in _SpritesToFlash)
        {
            s.material = new Material(s.material);
        }
    }

    public void AddUIToFlash(Image image) //used in event
    {
        image.material = new Material(image.material);
        _ImagesToFlash.Add(image);
    }
    #endregion FlashOnHit
}
