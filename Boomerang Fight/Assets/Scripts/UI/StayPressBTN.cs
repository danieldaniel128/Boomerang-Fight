using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StayPressBTN : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnPressBTN;
    private bool _isPressed;
    private void Update()
    {
        if (_isPressed)
            OnPressBTN?.Invoke();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isPressed = false;
    }

    
}
