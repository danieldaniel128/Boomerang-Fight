using System;
using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class UserInputPhone : ScriptableObject, IPhoneInput // interface "IPhoneInput" that has GetTouchPhase, GetTouchPosition
{
    public Vector2 TouchPosition;
    public TouchPhase TouchPhase;

    public TouchPhase GetTouchPhase()
    {
        return TouchPhase.None;
    }

    public Vector2 GetTouchPosition()
    {
       return TouchPosition;
    }

    //phase
}
