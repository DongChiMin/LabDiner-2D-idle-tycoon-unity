using LabDiner.LevelSystem.Domain;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using LabDiner.Shared.SO;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CheatPanel _panel;

    [Header("Events")]
    [SerializeField] private UIPopupEvent _onPopupShow;
    [SerializeField] private ClickOutsideEffect _clickOutsideEffect;

    [Header("Data")]
    [SerializeField] private DoubleRuntimeSO _coinData;
    [SerializeField] private IntRuntimeSO _gemData;

    [Header("Coin Currency")]
    [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;
    [SerializeField] private Transform _coinFlyStartPos;
    [SerializeField] private string _fixedCoinValue = "10000";
    [SerializeField] private TMP_InputField _inputCoinValue;
    [SerializeField] private Button _btnAddCoinValue;
    [SerializeField] private Button _btnAddFixedCoin;

    [Header("Gem Currency")]
    [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;
    [SerializeField] private Transform _gemFlyStartPos;
    [SerializeField] private int _fixedGemValue = 100;
    [SerializeField] private TMP_InputField _inputGemValue;
    [SerializeField] private Button _btnAddGemValue;
    [SerializeField] private Button _btnAddFixedGem;

    [Header("Level")]
    [SerializeField] private LevelConfigEvent _onLevelComplete;
    [SerializeField] private Button _btnSkipCurrentLevel;
    [SerializeField] private Button _btnSkipToValueLevel;

    [Header("Level Load")]
    [SerializeField] private LevelRegistrySO _levelRegistry;
    [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;
    
    bool isFetchProgress = false;
    void FetchProgress()
    {
        PlayerSave progress = _progressRuntimeSO.PlayerSave;
        _currentLevelConfigSO = _levelRegistry.GetConfigByID(progress.CurrentLevelID);
    }

    void OnEnable()
    {
        _onPopupShow.Register(HandlePopupShow);
        _clickOutsideEffect.OnClickOutside += HandlePopupHide;
        _panel.CloseButton.onClick.AddListener(HandlePopupHide);
        _btnAddCoinValue.onClick.AddListener(HandleAddCoinValue);
        _btnAddFixedCoin.onClick.AddListener(HandleAddFixedCoin);
        _btnAddGemValue.onClick.AddListener(HandleAddGemValue);
        _btnAddFixedGem.onClick.AddListener(HandleAddFixedGem);
        _btnSkipCurrentLevel.onClick.AddListener(HandleSkipCurrentLevel);
        _btnSkipToValueLevel.onClick.AddListener(HandleSkipToValueLevel);
    }

    void OnDisable()
    {
        _onPopupShow.Unregister(HandlePopupShow);
        _clickOutsideEffect.OnClickOutside -= HandlePopupHide;
        _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);
        _btnAddCoinValue.onClick.RemoveListener(HandleAddCoinValue);
        _btnAddFixedCoin.onClick.RemoveListener(HandleAddFixedCoin);
        _btnAddGemValue.onClick.RemoveListener(HandleAddGemValue);
        _btnAddFixedGem.onClick.RemoveListener(HandleAddFixedGem);
        _btnSkipCurrentLevel.onClick.RemoveListener(HandleSkipCurrentLevel);
        _btnSkipToValueLevel.onClick.RemoveListener(HandleSkipToValueLevel);
    }

    private void HandlePopupShow()
    {
        _panel.Setup();
        _panel.Show();
    }

    private void HandlePopupHide()
    {
        _panel.Hide();
    }

    #region Handle Currency

    private void HandleAddCoinValue()
    {
        double value = CurrencyFormatter.Format(_inputCoinValue.text);

            CoinRewardData data = new CoinRewardData
                {
                    startPos = _coinFlyStartPos.position,  
                    RewardValue = value
                };
                _onCoinFlyAdded.Raise(data);
        
    }

    private void HandleAddFixedCoin()
    {
        double value = CurrencyFormatter.Format(_fixedCoinValue);
        CoinRewardData data = new CoinRewardData
            {
                startPos = _coinFlyStartPos.position,
                RewardValue = value
            };
            _onCoinFlyAdded.Raise(data);
    }

    private void HandleAddGemValue()
    {
        if (int.TryParse(_inputGemValue.text, out int value))
        {
            GemRewardData data = new GemRewardData
                {
                    startPos = _gemFlyStartPos.position,  
                    RewardValue = value
                };
                _onGemFlyAdded.Raise(data);
        }
    }

    private void HandleAddFixedGem()
    {
        GemRewardData data = new GemRewardData
            {
                startPos = _gemFlyStartPos.position,
                RewardValue = (int)_fixedGemValue
            };
            _onGemFlyAdded.Raise(data);
    }


    #endregion

    #region Handle Level

    private LevelConfigSO _currentLevelConfigSO;
    private void HandleSkipCurrentLevel()
    {
        if(!isFetchProgress)
        {
            FetchProgress();
            isFetchProgress = true;
        }
        _onLevelComplete.Raise(_currentLevelConfigSO);
        _panel.Hide();
    }

    private void HandleSkipToValueLevel()
    {
        if(!isFetchProgress)
        {
            FetchProgress();
            isFetchProgress = true;
        }
        // Implementation for skipping level with value
    }

    #endregion
}
