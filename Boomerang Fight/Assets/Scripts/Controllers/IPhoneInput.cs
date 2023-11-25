using UnityEngine;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public interface IPhoneInput
{
    public TouchPhase GetTouchPhase();
    public Vector2 GetTouchPosition();
}