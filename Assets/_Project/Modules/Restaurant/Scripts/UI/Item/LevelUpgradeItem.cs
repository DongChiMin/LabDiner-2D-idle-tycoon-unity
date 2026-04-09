using LabDiner.Shared.SO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace LabDiner.Restaurant
{
    public class LevelUpgradeItem : MonoBehaviour
    {         
        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _upgradeTypeImage;
        [SerializeField] private Button _upgradeButton;

        private double _upgradeCost;

        void OnEnable()
        {
            _onCoinUpdated.Register(HandleCoinUpdated);
        }
        
        void OnDisable()
        {
            _onCoinUpdated.Unregister(HandleCoinUpdated);
        }

        #region API

        public void Init(BaseUpgradeSO upgradeSO)
        {
            _titleText.text = upgradeSO.Title;
            _descriptionText.text = upgradeSO.Description;
            _costText.text = upgradeSO.UpgradeCost.ToString("F0");
            _iconImage.sprite = upgradeSO.Icon;
            _upgradeTypeImage.sprite = upgradeSO.UpgradeTypeSprite;

            _upgradeCost = upgradeSO.UpgradeCost;

            double playerCoins = LevelManagerContext.Instance.LevelCurrencyManager.CurrentCoin;
            if (_upgradeCost > playerCoins)
            {
                _upgradeButton.interactable = false;
            }
            else
            {
                _upgradeButton.interactable = true;
            }

             _upgradeButton.onClick.AddListener(() => 
             {
                _onCoinSpent.Raise(_upgradeCost);
                upgradeSO.ApplyUpgrade();
             });
        }

       #endregion

        private void HandleCoinUpdated(double currentCoin)
        {
            if (currentCoin >= _upgradeCost)
            {
                _upgradeButton.interactable = true;
            }
            else
            {
                _upgradeButton.interactable = false;
            }
        }
    }
}
