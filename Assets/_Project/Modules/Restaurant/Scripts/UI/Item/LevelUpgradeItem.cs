using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Extension;

namespace LabDiner.Restaurant.UI
{
    public class LevelUpgradeItem : MonoBehaviour
    {
        public BaseUpgradeSO UpgradeSO => _baseUpgradeSO;
        public Button UpgradeButton => _upgradeButton;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _upgradeTypeImage;

        [Header("Button Settings")]
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Image _buttonCoinIcon;
        [SerializeField] private TextMeshProUGUI _costText;

        private BaseUpgradeSO _baseUpgradeSO;
        private bool _isInitColor = false;
        private Color _buttonTextColorOn;
        private Color _buttonIconColorOn;

        #region API

        public void Init(BaseUpgradeSO upgradeSO)
        {
            _baseUpgradeSO = upgradeSO;

            _titleText.text = upgradeSO.Title;
            _descriptionText.text = upgradeSO.Description;
            _costText.text = CurrencyFormatter.Format(upgradeSO.UpgradeCost);
            _iconImage.sprite = upgradeSO.Icon;
            _upgradeTypeImage.sprite = upgradeSO.UpgradeTypeSprite;
        }

        private void InitColor()
        {
            _buttonTextColorOn = _costText.color;
            _buttonIconColorOn = _buttonCoinIcon.color;
        }

        public void ToggleUpgradeButton(bool isOn)
        {

            if (!_isInitColor)
            {
                _isInitColor = true;
                InitColor();
            }
            _upgradeButton.interactable = isOn;
            if (!isOn)
            {
                _costText.color = _upgradeButton.colors.disabledColor;
                _buttonCoinIcon.color = _upgradeButton.colors.disabledColor;
            }
            else
            {
                _costText.color = _buttonTextColorOn;
                _buttonCoinIcon.color = _buttonIconColorOn;
            }
        }

        #endregion
    }
}
