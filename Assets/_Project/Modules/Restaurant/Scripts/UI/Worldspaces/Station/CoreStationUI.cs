using System;
using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public struct CoreStationUIData
    {
        public int CurrentLevel;
        public string Name;

        public int StarQuantity;
        public float StarProgress;

        public double CurrentProfit;
        public double CurrentCost;
        public float CurrentProcessTime;
    }
    public class CoreStationUI : BasePanel
    {
        public Action OnUpgradeButtonClicked;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinUpdated;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _starProgressFill;
        [SerializeField] private TextMeshProUGUI _profitText;
        [SerializeField] private TextMeshProUGUI _processTimeText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _upgradeButton;

        [Header("Effect")]
        [SerializeField] private PopScaleEffect _popScaleEffect;
        [SerializeField] private ClickOutsideEffect _clickOutsideEffect;

        // Internal state
        private double _currentCost;
        private bool _isMaxLevel = false;

        void OnEnable()
        {
            _onCoinUpdated.Register(OnCoinUpdated);
            _clickOutsideEffect.OnClickOutside += HandleClickOutside;
        }

        void OnDisable()
        {
            _onCoinUpdated.Unregister(OnCoinUpdated);
            _clickOutsideEffect.OnClickOutside -= HandleClickOutside;
        }

        void Awake()
        {
            _upgradeButton.onClick.AddListener(() => OnUpgradeButtonClicked?.Invoke());
        }

        #region API
        public void MaxLevelReached()
        {
            _isMaxLevel = true;

            // Cập nhật UI để hiển thị trạng thái đã đạt cấp độ tối đa
            ToggleUpgradeButton(false);
            _levelText.text = $"Lvl MAX";
            _costText.text = "Lvl MAX";
            _starProgressFill.fillAmount = 1f;
        }

        public void Setup(CoreStationUIData data)
        {
            if(_isMaxLevel)
            {
                return;
            }

            _currentCost = data.CurrentCost;

            _nameText.text = data.Name;
            _levelText.text = $"Lvl {data.CurrentLevel}";
            _profitText.text = $"{data.CurrentProfit:F0}";
            _processTimeText.text = $"{data.CurrentProcessTime:F1}";
            _costText.text = $"${data.CurrentCost:F0}";

            _starProgressFill.fillAmount = data.StarProgress;

            double currentCoin = LevelManagerContext.Instance.LevelCurrencyManager.CurrentCoin;
            ToggleUpgradeButton(currentCoin >= data.CurrentCost);
        }

        public override void Show()
        {
            _popScaleEffect.Show();
        }

        public override void Hide(Action onComplete = null)
        {
                _popScaleEffect.Hide(() =>
                {
                    onComplete?.Invoke();
                });
        }

        #endregion

        private void HandleClickOutside()
        {
            Hide();
        }

        private void OnCoinUpdated(double newCoinAmount)
        {
            if(_isMaxLevel)
            {
                return;
            }
            ToggleUpgradeButton(newCoinAmount >= _currentCost);
        }

        private void ToggleUpgradeButton(bool canUpgrade)
        {
            _upgradeButton.interactable = canUpgrade;
        }
    }
}
