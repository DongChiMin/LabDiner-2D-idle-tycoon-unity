
using System.Collections.Generic;
using LabDiner.Shared.Input;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class CoreStationUIController : MonoBehaviour, IInteractable
    {
        
        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinUpdated;

        [Header("View")]
        [SerializeField] private CoreStationUI _CoreStationUI;
        [SerializeField] private CoreStationStarUI _CoreStationStarUI;
        [SerializeField] private CoreStationUnlockUI _CoreStationUnlockUI;
        [SerializeField] private CoreStationToastUI _coreStationToastUI;

        [Header("Model")]
        [SerializeField] private CoreStation _CoreStation;

        private void OnEnable()
        {
            _CoreStation.OnDataChanged += UpdateUI;
            _CoreStation.OnMaxLevel += HandleMaxLevelReached;

            _onCoinUpdated.Register(HandleCoinUpdated);

            _CoreStationUI.OnUpgradeButtonClicked += RequestUpgrade;
            _CoreStationUnlockUI.OnUpgradeButtonClicked += RequestUpgrade;
        }

        private void OnDisable()
        {
            _CoreStation.OnDataChanged -= UpdateUI;
            _CoreStation.OnMaxLevel -= HandleMaxLevelReached;

            _onCoinUpdated.Unregister(HandleCoinUpdated);

            _CoreStationUI.OnUpgradeButtonClicked -= RequestUpgrade;
            _CoreStationUnlockUI.OnUpgradeButtonClicked -= RequestUpgrade;
        }

        #region API

        public void ShowUpgradeEffect(string effectText)
        {
            _coreStationToastUI.Show(effectText);
        }

        #endregion

        #region IInteractable Implementation
        //Khi click vào trạm:
        // Nếu chưa mở khóa: Hiện UI mở khóa, hiển thị thông tin về trạm và yêu cầu người chơi chi tiền để mở khóa
        // Nếu đã mở khóa: Hiện UI nâng cấp, hiển thị thông tin về trạm, lợi nhuận hiện tại, chi phí nâng cấp và tiến trình sao. Cho phép người chơi nâng cấp trạm nếu có đủ tiền.
        public void OnInteract()
        {
            if (!_CoreStation.IsUnlocked)
            {
                _CoreStationUnlockUI.Show();
                return;
            }
            else
            {
                _CoreStationUI.Show();
            }
        }

        /// <summary>
        /// trả về true => có thể click vào
        /// </summary>
        /// <returns></returns>
        public bool CanInteract()
        {
            return true;
        }

        #endregion


        #region Private Methods
        private void UpdateUI()
        {
            CoreStationUIData data = _CoreStation.GetUIData();
            bool isUnlocked = _CoreStation.IsUnlocked;

            _CoreStationStarUI.Setup(data);
            _CoreStationUI.Setup(data);
            if(!isUnlocked)
            {
                _CoreStationUnlockUI.Setup(data);
            }
            else
            {
                _CoreStationUnlockUI.Hide();
            }
        }

        private void HandleCoinUpdated(double currentCoin)
        {
            UpdateUI();
        }

        private void RequestUpgrade()
        {
            _CoreStation.Upgrade();
        }

        private void HandleMaxLevelReached(int maxStar)
        {
            _CoreStationUI.MaxLevelReached();
            _CoreStationStarUI.MaxLevelReached(maxStar);
        }
        #endregion

    }
}