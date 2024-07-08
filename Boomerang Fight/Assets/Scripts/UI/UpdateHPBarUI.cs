using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpdateHPBarUI : MonoBehaviourPun
{
    [Header("Sliders")]
    [SerializeField] private Slider _enemyHealthBar_Slider;
    [SerializeField] private Slider _playerHealthBar_Slider;
    [Header("LocalPlayer")]
    [SerializeField] private GameObject _playerHealthBar;
    [SerializeField] private TMP_Text _playerlivesCount_TMP;
    [SerializeField] private TMP_Text _playerNickname;
    [Header("EnemyPlayer")]
    [SerializeField] private GameObject _enemyHealthBar;
    [SerializeField] private TMP_Text _enemyLivesCount_TMP;
    [SerializeField] private TMP_Text _enemyNickname;

    [Header("Utilities")]
    [SerializeField] Transform _bufferEnemyParent;
    [SerializeField] Transform _bufferPlayerParent;
    [SerializeField] Image _playerBufferPrefab;
    [SerializeField] Image _enemyBufferPrefab;
    [SerializeField] private Canvas _healthBar_Canvas;
    [SerializeField] private Health _playerHealth;
    [Header("Monitors")]
    [SerializeField] Image _bufferPrefab;
    [SerializeField] Transform _bufferParent;
    [SerializeField] private Slider _currentHealthBar_Slider;
    [SerializeField] private TMP_Text _currentLivesCount_TMP;

    public UnityEvent<Image> OnHandleCreated = new();

    // Function to update the health bar material
    private void Start()
    {
        SetComponentOfHealthBar();
        CreateBuffersToHealthBar();
        UpdateOnLivesCountChangedEvent(_playerHealth.LivesCount);
    }
    void SetComponentOfHealthBar()
    {
        _healthBar_Canvas.worldCamera = Camera.main;
        if (photonView.IsMine)
        {
            _playerHealthBar.SetActive(true);
            _playerNickname.text = photonView.Owner.NickName;
            _currentHealthBar_Slider = _playerHealthBar_Slider;
            _currentLivesCount_TMP = _playerlivesCount_TMP;
            _bufferParent = _bufferPlayerParent;
            _bufferPrefab = _playerBufferPrefab;
        }
        else
        {
            _enemyHealthBar.SetActive(true);
            _enemyNickname.text = photonView.Owner.NickName;
            _currentHealthBar_Slider = _enemyHealthBar_Slider;
            _currentLivesCount_TMP = _enemyLivesCount_TMP;
            _bufferParent = _bufferEnemyParent;
            _bufferPrefab = _enemyBufferPrefab;
        }
    }
    void CreateBuffersToHealthBar()
    {
        for (int i = 0; i < _playerHealth.MaxHP - 1; i++)
        {
            Image newHandle = Instantiate(_bufferPrefab, _bufferParent);
            OnHandleCreated.Invoke(newHandle);
            //add divider to ui to flash
        }
    }
    public void UpdateOnHealthChangedEvent(float newHP,float maxHP)
    {
        _currentHealthBar_Slider.value = Mathf.Clamp01(newHP / maxHP);
    }
    public void UpdateOnLivesCountChangedEvent(int livesCount)
    {
        _currentLivesCount_TMP.text = livesCount.ToString();
    }
}
