using LabDiner.Shared.SO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace LabDiner.Restaurant
{
    public class LevelUpgradeItem : MonoBehaviour
    {         
        public BaseUpgradeSO UpgradeSO => _baseUpgradeSO;
        public Button UpgradeButton => _upgradeButton;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _upgradeTypeImage;
        [SerializeField] private Button _upgradeButton;

        private BaseUpgradeSO _baseUpgradeSO;

        #region API

        public void Init(BaseUpgradeSO upgradeSO)
        {
            _baseUpgradeSO = upgradeSO;

            _titleText.text = upgradeSO.Title;
            _descriptionText.text = upgradeSO.Description;
            _costText.text = upgradeSO.UpgradeCost.ToString("F0");
            _iconImage.sprite = upgradeSO.Icon;
            _upgradeTypeImage.sprite = upgradeSO.UpgradeTypeSprite;
        }

        public void ToggleUpgradeButton(bool isOn)
        {
            _upgradeButton.interactable = isOn;
        }

       #endregion
    }
}
