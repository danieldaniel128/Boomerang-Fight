using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyOffScreenPointer : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] RectTransform _pointerRectTransform;
    [SerializeField] float _xOffsetFromEdge = 200f;
    [SerializeField] float _yOffsetFromEdge = 100f;
    [SerializeField] float _minScale = 0.7f;
    [SerializeField] float _maxScale = 1f;
    [SerializeField] float _minScaleDistance;
    [SerializeField] float _maxScaleDistance;

    Transform _playerTransform;
    OnlinePlayer _targetPlayer;
    Vector3 PlayerPosition => new Vector3(_playerTransform.position.x, _playerTransform.position.z, 0);
    Vector3 TargetPosition => new Vector3(_targetPlayer.transform.position.x, _targetPlayer.transform.position.z, 0);
    Vector3 TargetScreenPosition => Camera.main.WorldToScreenPoint(TargetPosition);
    public OnlinePlayer TargetPlayer => _targetPlayer;

    public void Initialize(Transform playerTransform, OnlinePlayer targetPlayer)
    {
        this._targetPlayer = targetPlayer;
        this._playerTransform = playerTransform;
    }

    public void UpdatePointer()
    {
        //check if target is off screen
        if (!CheckIfTargetOffScreen())
        {
            DeactivatePointer();
            return;
        }

        ActivatePointer();
        SetPointerAngle();
        SetPointerPosition();
    }

    private void SetPointerPosition()
    {
        Vector3 screenClampedTargetPosition = new Vector3(Mathf.Clamp(TargetScreenPosition.x, _xOffsetFromEdge, Screen.width - _xOffsetFromEdge), Mathf.Clamp(TargetScreenPosition.y, _yOffsetFromEdge, Screen.height - _yOffsetFromEdge));
        _pointerRectTransform.position = screenClampedTargetPosition;
    }

    private bool CheckIfTargetOffScreen()
    {
        return TargetScreenPosition.x <= 0 || TargetScreenPosition.x >= Screen.width || TargetScreenPosition.y <= 0 || TargetScreenPosition.y >= Screen.height;
    }

    private void SetPointerAngle()
    {
        //direction from player to enemy.
        Vector3 dir = (TargetPosition - PlayerPosition).normalized;

        //rotate pointer towards target.
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        if (angle < 0) angle += 360;
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void ActivatePointer()
    {
        if (_image.enabled) return;
        _image.enabled = true;
    }
    public void DeactivatePointer()
    {
        if (!_image.enabled) return;
        _image.enabled = false;
    }
}
