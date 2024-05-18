using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateHPBarUI : MonoBehaviour
{
    [SerializeField] private Slider _healthBar_Slider;
    [SerializeField] private TMP_Text _livesCount_TMP;
    [SerializeField] private Canvas _healthBar_Canvas;
    // Function to update the health bar material
    private void Start()
    {
        _healthBar_Canvas.worldCamera = Camera.main;
    }
    public void UpdateOnHealthChangedEvent(float newHP,float maxHP)
    {
        _healthBar_Slider.value = Mathf.Clamp01(newHP / maxHP);
    }
    public void UpdateOnLivesCountChangedEvent(int livesCount)
    {
        _livesCount_TMP.text = livesCount.ToString();
    }
}
