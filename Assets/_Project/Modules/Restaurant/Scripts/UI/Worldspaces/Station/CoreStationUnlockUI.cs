using System;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class CoreStationUnlockUI : MonoBehaviour
    {
        public Action OnUpgradeButtonClicked;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Button _upgradeButton;

        [Header("Effect")]
        [SerializeField] private PopScaleEffect _popScaleEffect;
        [SerializeField] private ClickOutsideEffect _clickOutsideEffect;

        // Internal state
        [Header("[DEBUG]")]
        [SerializeField] private double _currentCost;

        private bool _isClicked = false;

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
            _upgradeButton.onClick.AddListener(() =>
            {
                if(_isClicked) return; // Prevent multiple clicks

                _isClicked = true;
                _upgradeButton.interactable = false;
                OnUpgradeButtonClicked?.Invoke();
            });
        }

        #region API

        public void Setup(CoreStationUIData data)
        {
            _currentCost = data.CurrentCost;

            _nameText.text = data.Name;
            _costText.text = $"${data.CurrentCost:F0}";

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
