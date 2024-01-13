using System;
using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public abstract class UserInputPhone : ScriptableObject, IPhoneInput // interface "IPhoneInput" that has GetTouchPhase, GetTouchPosition
{
    public Vector2 TouchPosition;
    public TouchPhase TouchPhase;
    public abstract void SubscribeInputActions();
    public abstract void UnsubscribeInputActions();
    public virtual TouchPhase GetTouchPhase()
    {
        return TouchPhase.None;
    }

    public virtual Vector2 GetTouchPosition()
    {
       return TouchPosition;
    }

    //phase
}
