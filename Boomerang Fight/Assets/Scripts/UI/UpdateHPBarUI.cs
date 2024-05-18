using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHPBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar_Slider;
    [SerializeField] private Canvas healthBar_Canvas;
    // Function to update the health bar material
    private void Start()
    {
        healthBar_Canvas.worldCamera = Camera.main;
    }
    public void UpdateOnHealthChangedEvent(float newHP,float maxHP)
    {
        healthBar_Slider.value = Mathf.Clamp01(newHP / maxHP);
    }
}
