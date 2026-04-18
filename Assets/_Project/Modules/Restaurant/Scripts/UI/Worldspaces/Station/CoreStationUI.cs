using System;
using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using LabDiner.Shared.UI;
using LabDiner.UI;
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
        public int MaxStar;
        public float StarProgress;
        public List<Sprite> StarRewardIcons;

        public double CurrentProfit;
        public double CurrentCost;
        public float CurrentProcessTime;
        public bool CanUpgrade;
    }
    public class CoreStationUI : MonoBehaviour
    {
        public Action OnUpgradeButtonClicked;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _profitText;
        [SerializeField] private TextMeshProUGUI _processTimeText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private HoldableButton _upgradeButton;

        [Header("Effect")]
        [SerializeField] private PopScaleEffect _popScaleEffect;
        [SerializeField] private ClickOutsideEffect _clickOutsideEffect;

        // Internal state
        private double _currentCost;
        private bool _isMaxLevel = false;

        void OnEnable()
        {
            _clickOutsideEffect.OnClickOutside += HandleClickOutside;
        }

        void OnDisable()
        {
            _clickOutsideEffect.OnClickOutside -= HandleClickOutside;
        }

        void Awake()
        {
            _upgradeButton.OnTurboTick += () => OnUpgradeButtonClicked?.Invoke();
        }

        #region API
        public void MaxLevelReached()
        {
            _isMaxLevel = true;

            // Cập nhật UI để hiển thị trạng thái đã đạt cấp độ tối đa
            ToggleUpgradeButton(false);
            _levelText.text = $"Lvl MAX";
            _costText.text = "Lvl MAX";
        }

        public void Setup(CoreStationUIData data)
        {
            if(_isMaxLevel)
            {
                return;
            }

            string formattedProfit = CurrencyFormatter.Format(data.CurrentProfit);
            string formattedCost = CurrencyFormatter.Format(data.CurrentCost);

            _currentCost = data.CurrentCost;

            _nameText.text = data.Name;
            _levelText.text = $"Lvl {data.CurrentLevel}";
            _profitText.text = formattedProfit;
            _processTimeText.text = $"{data.CurrentProcessTime:F1}";
            _costText.text = formattedCost;

            ToggleUpgradeButton(data.CanUpgrade);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _popScaleEffect.Show();
        }

        public void Hide(Action onComplete = null)
        {
                _popScaleEffect.Hide(() =>
                {
                    onComplete?.Invoke();
                    gameObject.SetActive(false);
                });
        }

        #endregion

        private void HandleClickOutside()
        {
            Hide();
        }

        private void ToggleUpgradeButton(bool canUpgrade)
        {
            _upgradeButton.interactable = canUpgrade;
        }
    }
}
